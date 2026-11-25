namespace Cubase.Macro.Services.Lex
{
    // cubase macro language (cl) services 
    public interface ICLService
    {
        void ProcessMacro(string text, Action<string> onError);
    }
}
