using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Midi;
using System.Diagnostics;
using System.Reflection;

namespace Cubase.Macro.Services.Lex
{
    public class ScriptFunctionService : IScriptFunctionService
    {
        private Dictionary<string, Func<FunctionCallExpression, LexVariable?, bool>> functions;

        public IEnumerable<string> Functions => functions.Select(x => x.Key);

        private Action<string>? OnError;

        private Action<string>? OnProgress;

        private IMidiService midiService;

        private CubaseKeyCommandCollection midiCommands;

        public ScriptFunctionService(IMidiService midiService)
        {
            this.functions = new Dictionary<string, Func<FunctionCallExpression, LexVariable?, bool>>()
            {
                {"RecordEnableByName",  this.RecordEnableByName},
                {"RecordDisableByName",  this.RecordDisableByName},
                {"MuteEnableByName",  this.MuteEnableByName},
                {"MuteDisableByName",  this.MuteDisableByName},
                {"SoloEnableByName",  this.SoloEnableByName},
                {"SoloDisableByName",  this.SoloDisableByName},
                {"SelectEnableByName", this.SelectEnableByName },
                {"SelectDisableByName", this.SelectDisableByName },
                {"SetSelected", this.SetSelected },
                {"SetTransport", this.SetTransport },
                {"ExecuteMidi", this.ExecuteMidi }
            };

            this.midiService = midiService;
            this.InitialiseMidiCommands();

        }

        public CubaseScriptCommandCollection GetScriptCommandsInfo()
        {
            var methodInfoCollection = new CubaseScriptCommandCollection();
            var entries = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(m => (Method: m, Attr: m.GetCustomAttribute<CubaseScriptMethodAttribute>(inherit: false)))
                .Where(x => x.Attr is not null);
            foreach (var methodInfo in entries)
            {
                if (methodInfo.Attr != null)
                {
                    methodInfoCollection.Add(new CubaseScriptCommand()
                    {
                        Name = methodInfo.Attr.Name,
                        Description = methodInfo.Attr.Description,
                        Example = methodInfo.Attr.Example
                    });
                }
            }
            return methodInfoCollection;
        }

        public bool ExecuteFunction(FunctionCallExpression functionCall, Action<string> onError, Action<string> onProgress, LexVariable? variable = null)
        {
            this.OnError = onError;
            this.OnProgress = onProgress;
            var func = this.functions.FirstOrDefault(x => x.Key.Equals(functionCall.Name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(func.Key))
            {
                this.OnProgress($"Executing function {func.Key}");
                return this.functions[func.Key](functionCall, variable);
            }
            onError?.Invoke($"Could not find function {functionCall.Name}");
            return false;
        }

        [CubaseScriptMethod("RecordEnableByName", "Enable Record on Specified Tracks", "var boolResult = RecordEnableByName(\"Track1\",\"Track2\")")]
        private bool RecordEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Record, variable);
        }

        [CubaseScriptMethod("RecordDisableByName", "Disable Record on Specified Tracks", "var boolResult = RecordDisableByName(\"Track1\",\"Track2\")")]

        private bool RecordDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Record, variable);
        }

        [CubaseScriptMethod("MuteEnableByName", "Enable Mute on Specified Tracks", "var boolResult = MuteEnableByName(\"Track1\",\"Track2\")")]
        private bool MuteEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Mute, variable);
        }

        [CubaseScriptMethod("MuteDisableByName", "Disable Mute on Specified Tracks", "var boolResult = MuteDisableByName(\"Track1\",\"Track2\")")]

        private bool MuteDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Mute, variable);
        }

        [CubaseScriptMethod("SoloEnableByName", "Enable Mute on Specified Tracks", "var boolResult = SoloEnableByName(\"Track1\",\"Track2\")")]

        private bool SoloEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Solo, variable);
        }

        [CubaseScriptMethod("SoloDisableByName", "Disable Mute on Specified Tracks", "var boolResult = SoloDisableByName(\"Track1\",\"Track2\")")]

        private bool SoloDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Solo, variable);
        }

        [CubaseScriptMethod("SelectEnableByName", "Select a SINGLE track - Can Only Select one", "var boolResult = SelectEnableByName(\"Track1\")")]

        private bool SelectEnableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 1, TrackUpdateValueType.Selected, variable, false);
        }

        [CubaseScriptMethod("SelectDisableByName", "DeSelect one or more tracks", "var boolResult = SelectDisableByName(\"Track1\",\"Track2\") \\\\ OR use \"*\" to deselect all selected tracks")]

        private bool SelectDisableByName(FunctionCallExpression functionCall, LexVariable? variable)
        {
            return EnableDisableTrackValueTypesByName(functionCall, 0, TrackUpdateValueType.Selected, variable, false);
        }

        [CubaseScriptMethod("SetTransport", "Set a transport button (Play, Stop, PunchIn, PunchOut etc) to true (on) or false (off) ", "var boolResult = SetTransport(\"Play\",true)")]
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

        [CubaseScriptMethod("SetSelected", "Sets an action (Mute, Solo, Record) to on/off on ALL selected tracks", "var boolResult = SetSelected(\"Record\",true)")]
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

        [CubaseScriptMethod("ExecuteMidi", "Executes one or more Midi commands. The second parameter is the number of times to execute the command ", "var boolResult = ExecuteMidi(\"Cursor Back 1 Bar\",4)")]
        private bool ExecuteMidi(FunctionCallExpression functionCall, LexVariable? variable)
        {
            var midiCommandParameters = this.GetFunctionArgumentsAsString(functionCall);
            var iterationCount = 1;
            if (midiCommandParameters.Count == 2)
            {
                var haveCount = int.TryParse(midiCommandParameters.Last(), out iterationCount);
                if (haveCount)
                {
                    midiCommandParameters = new List<string> { midiCommandParameters.First() };
                }
            }
            for (int i = 0; i < iterationCount; i++)
            {
                foreach (var midiCommandName in midiCommandParameters)
                {
                    var midiCommand = this.midiCommands.GetByCategoryAndName(CubaseMacroConstants.Midi, midiCommandName);
                    if (midiCommand == null)
                    {
                        SetVariableToTrueOrFalse(variable, false);
                        this.OnError.Invoke($"Cannot execute Midi Command {midiCommandName}");
                        return false;
                    }
                    // set up confirmation handler 
                    var commandHandled = false;
                    this.midiService.OnMidiResponse = (midiResponse) =>
                    {
                        if (midiResponse.Name == midiCommand.Name) // maybe should check not and channel ?
                        {
                            commandHandled = true;
                        }
                    };
                    var result = this.midiService.SendMidiMessage(midiCommand, this.OnError);
                    if (!result)
                    {
                        SetVariableToTrueOrFalse(variable, false);
                        return false;
                    }
                    var responseResult = this.WaitUntil(() =>
                    {
                        return commandHandled;
                    }, TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(50));

                    if (!responseResult)
                    {
                        this.OnError.Invoke($"Did NOT receive a response from the midi command. {midiCommand.Name} within the 30 second response threshold");
                        SetVariableToTrueOrFalse(variable, false);
                        return false;
                    }
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

        private bool EnableDisableTrackValueTypesByName(FunctionCallExpression functionCall, int value, TrackUpdateValueType valueType, LexVariable? variable, bool checkForUpdate = true)
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
                    var state = this.UpdateTrack(valueType, track.Id, value, checkForUpdate);
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

        private bool UpdateTrack(TrackUpdateValueType valueType, string id, int value, bool checkForUpdate = false)
        {

            var asBoolValue = value == 0 ? false : true;

            this.midiService.SendSysExMessage<UpdateTrack>(Models.MidiCommand.UpdateTrack,
            new UpdateTrack()
            {
                Id = id,
                Value = value,
                TrackValueType = valueType
            });
            if (checkForUpdate)
            {
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
            else
            {
                return true;
            }
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
            if (trackNames.Count == 1 && trackNames.First() == "*")
            {
                trackNames = this.midiService.TrackCollection.GetSelectedTracks().Select(x => x.Name).ToList();
            }
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

        public CubaseKeyCommandCollection GetMidiCommands()
        {
            return this.midiCommands;
        }
    }
}
