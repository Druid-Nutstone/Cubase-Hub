namespace Cubase.Macro.Services.Lex
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CubaseScriptMethodAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Example { get; set; }

        public CubaseScriptMethodAttribute(string Name, string Description, string Example)
        {
            this.Name = Name;
            this.Description = Description;
            this.Example = Example;

        }
    }
}
