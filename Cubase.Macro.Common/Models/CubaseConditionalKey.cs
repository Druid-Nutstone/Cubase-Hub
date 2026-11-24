namespace Cubase.Macro.Common.Models
{
    public class CubaseConditionalKey
    {
        public List<CubaseCondition> Conditions { get; set; } = new List<CubaseCondition>();

        public CubaseKeyCommand Command { get; set; }
    }

    public class CubaseCondition
    {
        public string FunctionName { get; set; }

        public string PropertyName { get; set; }

        public bool State { get; set; }
    }
}
