using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Midi;
using System.Diagnostics;

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
                {"SetSelected", this.SetSelected },
                {"SetTransport", this.SetTransport },
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

        private bool SetTransport(FunctionCallExpression functionCall, LexVariable? variable)
        {
            var args = this.GetFunctionArgumentsAsString(functionCall);
            if (args.Count < 2)
            {
                SetVariableToTrueOrFalse(variable, false);
                this.OnError.Invoke("SetTransport requires two parameters. The action and true or false  ");
                return false;
            }
            var transportActionType = args[0].ToTransportValueType();
            var transportValueType = args[1].ToBooleanInt();
            // only change it if it's different 
            if (this.midiService.TransportModel.GetTransportValueFromTransportValueType(transportActionType) != transportValueType)
            {
                this.midiService.SendSysExMessage(MidiCommand.UpdateTransport, new UpdateTransport() { TransportValueType = transportActionType, Value = transportValueType });
                var state = this.WaitUntil(() =>
                {
                    switch (transportActionType)
                    {
                        case TransportUpdateValueType.Play:
                            return this.midiService.TransportModel.Play == transportValueType.IntToBool();
                        case TransportUpdateValueType.Stop:
                            return this.midiService.TransportModel.Stop == transportValueType.IntToBool();
                        case TransportUpdateValueType.Record:
                            return this.midiService.TransportModel.Record == transportValueType.IntToBool();
                        case TransportUpdateValueType.Cycle:
                            return this.midiService.TransportModel.Cycle == transportValueType.IntToBool();
                        case TransportUpdateValueType.PunchIn:
                            return this.midiService.TransportModel.PunchIn == transportValueType.IntToBool();
                        case TransportUpdateValueType.PunchOut:
                            return this.midiService.TransportModel.PunchOut == transportValueType.IntToBool();

                        default:
                            return false;
                    }
                }, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(100));
                if (!state)
                {
                    SetVariableToTrueOrFalse(variable, false);
                    this.OnError.Invoke($"Could NOT set {args[0]} value to {args[1]}. The operation timed out");
                    return false;
                }
            }

            SetVariableToTrueOrFalse(variable, true);
            return true;
        }
        private bool SetSelected(FunctionCallExpression functionCall, LexVariable? variable)
        {
            var selectedTracks = this.midiService.TrackCollection.GetSelectedTracks();
            if (selectedTracks.Count() == 0)
            {
                SetVariableToTrueOrFalse(variable, false);
                this.OnError.Invoke("There are no selected tracks");
                return false;
            }
            var args = this.GetFunctionArgumentsAsString(functionCall);

            if (args.Count < 2)
            {
                SetVariableToTrueOrFalse(variable, false);
                this.OnError.Invoke("SetSelected requires two parameters. The action and true or false  ");
                return false;
            }

            var trackActionType = args[0].ToTrackValueType();
            var trackValueType = args[1].ToBooleanInt();

            if (trackActionType == TrackUpdateValueType.Unknown)
            {
                SetVariableToTrueOrFalse(variable, false);
                this.OnError.Invoke($"{args[0]} is NOT a valid action on the track");
                return false;
            }

            foreach (var track in selectedTracks)
            {
                try
                {
                    // if it's already set - leave it 
                    if (track.GetTrackValueFromTrackValueType(trackActionType) != trackValueType)
                    {
                        var state = UpdateTrack(trackActionType, track.Id, trackValueType);
                        if (!state)
                        {
                            SetVariableToTrueOrFalse(variable, false);
                            this.OnError($"Could not set {args[0]} on track {track.Name}");
                            return state;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetVariableToTrueOrFalse(variable, false);
                    this.OnError($"Could not set {args[0]} on track {track.Name}. {ex.Message}");
                    return false;
                }
            }
            SetVariableToTrueOrFalse(variable, true);
            return true;
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
                try
                {
                    var state = this.UpdateTrack(valueType, track.Id, value);
                    if (!state)
                    {
                        this.SetVariableToTrueOrFalse(variable, false);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    this.OnError.Invoke(ex.Message);
                    this.SetVariableToTrueOrFalse(variable, false);
                    return false;
                }
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

        private bool UpdateTrack(TrackUpdateValueType valueType, string id, int value)
        {

            var asBoolValue = value == 0 ? false : true;

            this.midiService.SendSysExMessage<UpdateTrack>(Models.MidiCommand.UpdateTrack,
            new UpdateTrack()
            {
                Id = id,
                Value = value,
                TrackValueType = valueType
            });
            return this.WaitUntil(() =>
            {
                var track = this.midiService.TrackCollection.GetTrackById(id);
                if (track != null)
                {
                    switch (valueType)
                    {
                        case TrackUpdateValueType.Record:
                            return track.RecordEnable == asBoolValue;
                        case TrackUpdateValueType.Mute:
                            return track.Mute == asBoolValue;
                        case TrackUpdateValueType.Solo:
                            return track.Solo == asBoolValue;
                        case TrackUpdateValueType.Selected:
                            return track.Selected == asBoolValue;
                        default:
                            return false;
                    }
                }
                else
                {
                    throw new Exception($"Track with id {id} does not exist");
                }
            }, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(100));
        }


        private bool WaitUntil(
            Func<bool> condition,
            TimeSpan timeout,
            TimeSpan? pollInterval = null)
        {
            ArgumentNullException.ThrowIfNull(condition);

            if (condition())
            {
                return true;
            }

            TimeSpan interval = pollInterval ?? TimeSpan.FromMilliseconds(50);
            var stopwatch = Stopwatch.StartNew();

            Task<bool> task = Task.Run(() =>
            {
                while (stopwatch.Elapsed < timeout)
                {
                    if (condition())
                    {
                        return true;
                    }

                    TimeSpan remaining = timeout - stopwatch.Elapsed;
                    if (remaining <= TimeSpan.Zero)
                    {
                        break;
                    }

                    Thread.Sleep(remaining < interval ? remaining : interval);
                }

                return condition();
            });

            return task.GetAwaiter().GetResult();
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
