using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Services.ConditionalMacros
{
    public interface IConditionalMacroService
    {
        List<string> GetFunctionNames();

        List<string> GetFunctionProperties(string functionName);

        void Initialise();

        CubaseKeyCommand? ExecuteConditionalMacro(CubaseKeyCommand cubaseKeyCommand, Action<string> onError);
    }
}
