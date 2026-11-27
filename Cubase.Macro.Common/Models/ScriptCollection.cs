namespace Cubase.Macro.Common.Models
{
    public class ScriptCollection : List<ScriptInfo>
    {
    }

    public class ScriptInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ScriptPath { get; set; }
    }
}
