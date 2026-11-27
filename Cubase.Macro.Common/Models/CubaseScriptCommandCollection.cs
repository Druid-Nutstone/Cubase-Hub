namespace Cubase.Macro.Common.Models
{
    public class CubaseScriptCommandCollection : List<CubaseScriptCommand>
    {
    }

    public class CubaseScriptCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Example { get; set; }
    }
}
