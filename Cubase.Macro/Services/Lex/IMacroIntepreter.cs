namespace Cubase.Macro.Services.Lex
{
    public interface IMacroIntepreter
    {
        void ProcessStatements(List<Statement> statments, Action<string> onError, Action<string> onProgress);
    }
}
