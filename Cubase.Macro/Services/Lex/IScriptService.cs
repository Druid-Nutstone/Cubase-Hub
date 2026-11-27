using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lex
{
    // cubase macro language (cl) services 
    public interface IScriptService
    {
        void RunScript(string text, Action<string> onError, Action<string> onProgress);

        bool RunScriptFromFileName(string fileName, Action<string> onError, Action<string> onProgress);

        CubaseScriptCommandCollection GetScriptCommands();

        CubaseKeyCommandCollection GetMidiCommands();

        ScriptCollection GetScripts();
    }
}
