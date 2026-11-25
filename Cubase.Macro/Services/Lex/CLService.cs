namespace Cubase.Macro.Services.Lex
{
    public class CLService : ICLService
    {

        private readonly IMacroIntepreter intepreter;

        public CLService(IMacroIntepreter macroIntepreter)
        {
            this.intepreter = macroIntepreter;
        }

        public void ProcessMacro(string text, Action<string> onError)
        {
            var lexParser = new LexParser();
            var tokens = lexParser.Parse(text);
            var macroParser = new MacroParser(tokens);
            try
            {
                var statements = macroParser.Parse();
                this.intepreter.ProcessStatements(statements, onError);
            }
            catch (Exception ex)
            {
                onError.Invoke($"Error parsing script: {ex.Message}");
                return;
            }
        }
    }
}
