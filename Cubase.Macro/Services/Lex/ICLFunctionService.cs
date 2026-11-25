using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.Lex
{
    public interface ICLFunctionService
    {
        IEnumerable<string> Functions { get; }

        bool ExecuteFunction(FunctionCallExpression functionCall, Action<string> onError, LexVariable? variable = null);
    }
}
