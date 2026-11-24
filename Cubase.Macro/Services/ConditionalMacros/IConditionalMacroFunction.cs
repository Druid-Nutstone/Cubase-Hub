namespace Cubase.Macro.Services.ConditionalMacros
{
    public interface IConditionalMacroFunction
    {
        string FunctionName { get; }

        public IEnumerable<string> Properties { get; }


        public bool GetPropertyState(string propertyName);

        void Initialise();
    }
}
