using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lex
{
    public interface IScriptFunctionService
    {
        IEnumerable<string> Functions { get; }

        bool ExecuteFunction(FunctionCallExpression functionCall, Action<string> onError, Action<string> onProgress, LexVariable? variable = null);

        CubaseScriptCommandCollection GetScriptCommandsInfo();
        CubaseKeyCommandCollection GetMidiCommands();


    }
}
