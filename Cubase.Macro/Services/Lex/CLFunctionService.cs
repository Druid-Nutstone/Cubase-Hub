using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Midi;

namespace Cubase.Macro.Services.Lex
{
    public class CLFunctionService : ICLFunctionService
    {
        private Dictionary<string, Func<FunctionCallExpression, LexVariable?, bool>> functions;

        public IEnumerable<string> Functions => functions.Select(x => x.Key);

        private Action<string>? OnError;

        private IMidiService midiService;

        private CubaseKeyCommandCollection midiCommands;

        public CLFunctionService(IMidiService midiService)
        {
            this.functions = new Dictionary<string, Func<FunctionCallExpression, LexVariable?, bool>>()
            {
                {"RecordEnableByName",  this.RecordEnableByName},
                {"RecordDisableByName",  this.RecordDisableByName},
                {"MuteEnableByName",  this.MuteEnableByName},
                {"MuteDisableByName",  this.MuteDisableByName},
                {"SoloEnableByName",  this.SoloEnableByName},
                {"SoloDisableByName",  this.SoloDisableByName},
                {"ExecuteMidi", this.ExecuteMidi }
            };

            this.midiService = midiService;
            this.InitialiseMidiCommands();

        }

        public bool ExecuteFunction(FunctionCallExpression functionCall, Action<string> onError, LexVariable? variable = null)
        {
            this.OnError = onError;
            var func = this.functions.FirstOrDefault(x => x.Key.Equals(functionCall.Name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(func.Key))
            {
                return this.functions[func.Key](functionCall, variable);
            }
            onError?.Invoke($"Could not find function {functionCall.Name}");
            return false;
        }


        private bool RecordEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Record, variable);
        }

        private bool RecordDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Record, variable);
        }

        private bool MuteEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Mute, variable);
        }

        private bool MuteDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Mute, variable);
        }

        private bool SoloEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Solo, variable);
        }

        private bool SoloDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Solo, variable);
        }

        private bool ExecuteMidi(FunctionCallExpression functionCall, LexVariable? variable)
        {
            var midiCommandParameters = this.GetFunctionArgumentsAsString(functionCall);

            foreach (var midiCommandName in midiCommandParameters)
            {
                var midiCommand = this.midiCommands.GetByCategoryAndName(CubaseMacroConstants.Midi, midiCommandName);
                if (midiCommand == null)
                {
                    SetVariableToTrueOrFalse(variable, false);
                    this.OnError.Invoke($"Cannot execute Midi Command {midiCommandName}");
                    return false;
                }

                var result = this.midiService.SendMidiMessage(midiCommand, this.OnError);
                if (!result)
                {
                    SetVariableToTrueOrFalse(variable, false);
                    return false;
                }
            }
            SetVariableToTrueOrFalse(variable, true);
            return true;
        }

        private void InitialiseMidiCommands()
        {
            var allMidiCommands = new CubaseKeyCommandParser().Parse();
            this.midiCommands = allMidiCommands.GetFilteredBy(CubaseMacroConstants.Midi);
        }

        private bool EnableDisableTrackValueTypesByName(FunctionCallExpression functionCall, int value, TrackUpdateValueType valueType, LexVariable? variable)
        {
            var tracks = GetTracksByNameAndErrorIfNoMatch(functionCall);
            if (tracks == null)
            {
                this.SetVariableToTrueOrFalse(variable, false);
                return false;
            }

            foreach (var track in tracks)
            {
                this.UpdateTrack(valueType, track.Id, value);
            }
            this.SetVariableToTrueOrFalse(variable, true);
            return true;
        }

        private void SetVariableToTrueOrFalse(LexVariable? variable, bool trueOrFalse)
        {
            if (variable != null)
            {
                variable.Value = trueOrFalse;
                variable.ValueType = typeof(bool);
            }
        }

        private void UpdateTrack(TrackUpdateValueType valueType, string id, int value)
        {
            this.midiService.SendSysExMessage<UpdateTrack>(Models.MidiCommand.UpdateTrack,
            new UpdateTrack()
            {
                Id = id,
                Value = value,
                TrackValueType = valueType
            });
        }

        private List<Track>? GetTracksByNameAndErrorIfNoMatch(FunctionCallExpression functionCallExpression)
        {
            var trackNames = this.GetFunctionArgumentsAsString(functionCallExpression);
            var tracks = this.midiService.TrackCollection.GetTracksByName(trackNames);

            var foundNames = new HashSet<string>(
                tracks.Where(t => t.Name != null).Select(t => t.Name),
                StringComparer.OrdinalIgnoreCase);

            var missingNames = trackNames
                .Where(name => !string.IsNullOrEmpty(name) && !foundNames.Contains(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (missingNames.Count > 0)
            {
                this.OnError?.Invoke($"The following tracks do NOT exist in the current project. {string.Join(", ", missingNames)}");
                return null;
            }

            return tracks;
        }

        private List<string> GetFunctionArgumentsAsString(FunctionCallExpression functionCallExpression)
        {
            if (functionCallExpression?.Arguments == null)
            {
                return new List<string>();
            }

            return functionCallExpression.Arguments
                .OfType<LiteralExpression>()
                .Select(literal => literal.Value?.ToString() ?? string.Empty)
                .ToList();
        }

    }
}
