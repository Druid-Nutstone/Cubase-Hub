using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Window;
using System.IO;

namespace Cubase.Macro.Services.Lex
{
    public class ScriptService : IScriptService
    {

        private readonly IMacroIntepreter intepreter;

        private readonly IScriptFunctionService functionService;

        private IMidiService midiService;

        private IWindowService windowService;

        public ScriptService(IMacroIntepreter macroIntepreter,
                             IMidiService midiService,
                             IWindowService windowService,
                             IScriptFunctionService functionService)
        {
            this.intepreter = macroIntepreter;
            this.functionService = functionService;
            this.midiService = midiService;
            this.windowService = windowService;
            if (!Directory.Exists(CubaseMacroConstants.ScriptFolder))
            {
                Directory.CreateDirectory(CubaseMacroConstants.ScriptFolder);
            }
        }

        public ScriptService()
        {
        }

        public CubaseKeyCommandCollection GetMidiCommands()
        {
            return this.functionService.GetMidiCommands();
        }

        public CubaseScriptCommandCollection GetScriptCommands()
        {
            return this.functionService.GetScriptCommandsInfo();
        }

        public void RunScript(string text, Action<string> onError, Action<string> onProgress)
        {
            onProgress("Checking Envrionment ..");
            if (!this.PreFlightChecks(onError))
            {
                MessageBox.Show("Could not initialise script environment. Is Cubase running?");
                return;
            }
            var lexParser = new LexParser();
            onProgress("Converting Script text to tokens");
            var tokens = lexParser.Parse(text);
            var macroParser = new MacroParser(tokens);
            try
            {
                onProgress("Parsing Text and Running Script");
                var statements = macroParser.Parse();
                this.intepreter.ProcessStatements(statements, onError, onProgress);
            }
            catch (Exception ex)
            {
                onError.Invoke($"Error parsing script: {ex.Message}");
                return;
            }
        }

        private bool PreFlightChecks(Action<string> onError)
        {
            bool isInitialised = false;
            if (!this.windowService.IsCubaseActive(false))
            {
                return false;
            }

            if (!this.midiService.Initialised)
            {

                this.midiService.Initialise();

                int count = 0;
                var tsk = Task.Run(() =>
                {
                    while (!this.midiService.Initialised)
                    {
                        if (count > 10)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                        count++;
                    }
                    // but have we any tracks ?
                    count = 0;
                    while (this.midiService.TrackCollection.Count == 0)
                    {
                        if (count > 20)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                        count++;
                    }
                    isInitialised = this.midiService.TrackCollection.Count > 0;
                });
                tsk.Wait();
            }
            else
            {
                int count = 0;
                var tsk = Task.Run(() =>
                {
                    // but have we any tracks ?
                    count = 0;
                    while (this.midiService.TrackCollection.Count == 0)
                    {
                        if (count > 20)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                        count++;
                    }
                    isInitialised = this.midiService.TrackCollection.Count > 0;
                });
                tsk.Wait();
            }
            return isInitialised;
        }

        public ScriptCollection GetScripts()
        {
            // get scripts
            if (!Directory.Exists(CubaseMacroConstants.ScriptFolder))
            {
                return new ScriptCollection();
            }
            var scriptFiles = Directory.GetFiles(CubaseMacroConstants.ScriptFolder, $"*{CubaseMacroConstants.NutstoneCubaseScript}");
            var scriptCollection = new ScriptCollection();
            foreach (var scriptFile in scriptFiles)
            {
                var content = File.ReadAllText(scriptFile);
                var scriptParser = new MacroParser(new LexParser().Parse(content));
                var scriptInfo = scriptParser.ParseScriptMetaData();
                scriptCollection.Add(new ScriptInfo()
                {
                    Name = scriptInfo.Name,
                    Description = scriptInfo.Description,
                    ScriptPath = scriptFile
                });
            }
            return scriptCollection;
        }

        public bool RunScriptFromFileName(string fileName, Action<string> onError, Action<string> onProgress)
        {
            if (!File.Exists(fileName))
            {
                onError($"Script file {fileName} does not exist");
                return true;
            }
            var scriptText = File.ReadAllText(fileName);
            var lexParser = new LexParser();
            onProgress("Converting Script text to tokens");
            var tokens = lexParser.Parse(scriptText);
            var macroParser = new MacroParser(tokens);
            try
            {
                onProgress("Parsing Text and Running Script");
                var statements = macroParser.Parse();
                this.intepreter.ProcessStatements(statements, onError, onProgress);
                return true;
            }
            catch (Exception ex)
            {
                onError.Invoke($"Error parsing script: {ex.Message}");
                return false;
            }
        }
    }
}
