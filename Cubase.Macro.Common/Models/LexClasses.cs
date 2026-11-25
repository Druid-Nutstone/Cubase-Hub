namespace Cubase.Macro.Common.Models
{
    public class LexVariableCollection : List<LexVariable>
    {
        public LexVariable AddVariable(string name, object? value = null)
        {
            LexVariable variable = this.FirstOrDefault(x => x.Name == name);
            if (variable != null)
            {
                variable.Value = value;
            }
            else
            {
                variable = new LexVariable() { Name = name, Value = value };
                this.Add(variable);
            }
            return variable;
        }

        public LexVariable? GetVariableByName(string name)
        {
            return this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class LexVariable
    {
        public string Name { get; set; }

        public object? Value { get; set; }

        public Type ValueType { get; set; }
    }
}
