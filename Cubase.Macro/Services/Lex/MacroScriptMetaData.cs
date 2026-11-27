namespace Cubase.Macro.Services.Lex
{
    public class MacroScriptMetaData
    {
        public Dictionary<string, string> Metadata { get; } = new(StringComparer.OrdinalIgnoreCase);
        public List<Statement> Statements { get; } = new();

        public string? Name => Metadata.TryGetValue("Name", out var value) ? value : null;
        public string? Description => Metadata.TryGetValue("Description", out var value) ? value : null;
    }
}
