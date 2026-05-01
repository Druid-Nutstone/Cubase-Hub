using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cubase.Macro.Services.PLE
{
    /// <summary>
    /// Generic Cubase Project Logical Editor preset reader/writer.
    /// Uses XElement/XDocument so unknown nodes are preserved if desired.
    /// </summary>
    public class PleEditor
    {
        public string AppName { get; set; } = "Cubase";
        public int AppVersion { get; set; } = 15;

        public int VersionMagic { get; set; }
        public int VersionMagic12 { get; set; }

        public int ExpertSetting { get; set; }
        public string Info { get; set; } = "";
        public string PostName { get; set; } = "";

        public int ProcessType { get; set; }
        public int Context { get; set; }

        public int PreProcessors { get; set; }
        public int PostProcessors { get; set; }

        public List<PleObject> Actions { get; set; } = new();
        public List<PleObject> Filter { get; set; } = new();

        public List<string> PreCategory { get; set; } = new();
        public List<string> PreName { get; set; } = new();
        public List<int> PreState { get; set; } = new();

        public List<string> PostCategory { get; set; } = new();
        public List<string> PostNameList { get; set; } = new();
        public List<int> PostState { get; set; } = new();

        public string BinaryPresetHex { get; set; } = "";

        // --------------------------------------------------------
        // LOAD
        // --------------------------------------------------------
        public static PleEditor Load(string file)
        {
            return FromXDocument(XDocument.Load(file));
        }

        public static PleEditor FromXml(string xml)
        {
            return FromXDocument(XDocument.Parse(xml));
        }

        public static PleEditor FromXDocument(XDocument doc)
        {
            var p = new PleEditor();
            var root = doc.Root!;

            var version = root.Element("list");
            if (version != null)
            {
                var item = version.Element("item");
                if (item != null)
                {
                    p.AppName = GetString(item, "AppName");
                    p.AppVersion = GetInt(item, "AppVersion");
                }
            }

            var attr = root.Elements("member")
                           .FirstOrDefault(x => Attr(x, "name") == "Attributes");

            if (attr == null)
                return p;

            p.VersionMagic = GetInt(attr, "VersionMagic");
            p.VersionMagic12 = GetInt(attr, "VersionMagic12");

            p.ExpertSetting = GetNestedValue(attr, "ExpertSetting");
            p.ProcessType = GetNestedValue(attr, "processType");
            p.Context = GetNestedValue(attr, "Context");

            p.Info = GetNestedString(attr, "Info");
            p.PostName = GetNestedString(attr, "PostName");

            p.PreProcessors = GetInt(attr, "nPreProcessors");
            p.PostProcessors = GetInt(attr, "nPostProcessors");

            p.Actions = GetObjectList(attr, "Actions");
            p.Filter = GetObjectList(attr, "Filter");

            p.PreCategory = GetSimpleStringList(attr, "preCategory");
            p.PreName = GetSimpleStringList(attr, "preName");
            p.PreState = GetSimpleIntList(attr, "preState");

            p.PostCategory = GetSimpleStringList(attr, "postCategory");
            p.PostNameList = GetSimpleStringList(attr, "postName");
            p.PostState = GetSimpleIntList(attr, "postState");

            var bin = root.Elements("bin")
                          .FirstOrDefault(x => Attr(x, "name") == "Preset");

            if (bin != null)
                p.BinaryPresetHex = NormalizeHex(bin.Value);

            return p;
        }

        // --------------------------------------------------------
        // SAVE
        // --------------------------------------------------------
        public void Save(string file)
        {
            ToXDocument().Save(file);
        }

        public string ToXml()
        {
            return ToXDocument().ToString();
        }

        public XDocument ToXDocument()
        {
            var root = new XElement("Project_Logical_EditorPreset");

            root.Add(
                new XElement("list",
                    new XAttribute("name", "Version"),
                    new XAttribute("type", "list"),
                    new XElement("item",
                        StringNode("AppName", AppName),
                        IntNode("AppVersion", AppVersion)
                    )
                )
            );

            var attr = new XElement("member",
                new XAttribute("name", "Attributes"),

                IntNode("VersionMagic", VersionMagic),

                IntMember("ExpertSetting", ExpertSetting, 0, 1),
                StringMember("Info", Info),

                ObjectList("Actions", Actions),
                ObjectList("Filter", Filter),

                StringMember("PostName", PostName),

                IntMember("processType", ProcessType, 0, 8),
                IntMember("Context", Context, 0, 2),

                IntNode("VersionMagic12", VersionMagic12),
                IntNode("nPreProcessors", PreProcessors),
                IntNode("nPostProcessors", PostProcessors),

                StringList("preCategory", PreCategory),
                StringList("preName", PreName),
                IntList("preState", PreState),

                StringList("postCategory", PostCategory),
                StringList("postName", PostNameList),
                IntList("postState", PostState)
            );

            root.Add(attr);

            root.Add(
                new XElement("bin",
                    new XAttribute("name", "Preset"),
                    FormatHex(BinaryPresetHex))
            );

            return new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                root
            );
        }

        // --------------------------------------------------------
        // HELPERS
        // --------------------------------------------------------
        private static List<PleObject> GetObjectList(XElement parent, string listName)
        {
            var list = parent.Elements("list")
                .FirstOrDefault(x => Attr(x, "name") == listName);

            if (list == null) return new();

            return list.Elements("obj")
                       .Select(PleObject.FromXElement)
                       .ToList();
        }

        private static XElement ObjectList(string name, List<PleObject> objects)
        {
            return new XElement("list",
                new XAttribute("name", name),
                new XAttribute("type", "obj"),
                objects.Select(x => x.ToXElement()));
        }

        private static List<string> GetSimpleStringList(XElement parent, string name)
        {
            var list = parent.Elements("list")
                .FirstOrDefault(x => Attr(x, "name") == name);

            if (list == null) return new();

            return list.Elements("item")
                       .Select(x => Attr(x, "value"))
                       .ToList();
        }

        private static List<int> GetSimpleIntList(XElement parent, string name)
        {
            return GetSimpleStringList(parent, name)
                .Select(x => int.TryParse(x, out var v) ? v : 0)
                .ToList();
        }

        private static XElement StringList(string name, IEnumerable<string> values)
        {
            return new XElement("list",
                new XAttribute("name", name),
                new XAttribute("type", "string"),
                values.Select(v => new XElement("item",
                    new XAttribute("value", v ?? ""))));
        }

        private static XElement IntList(string name, IEnumerable<int> values)
        {
            return new XElement("list",
                new XAttribute("name", name),
                new XAttribute("type", "int"),
                values.Select(v => new XElement("item",
                    new XAttribute("value", v))));
        }

        private static XElement IntNode(string name, int value)
            => new XElement("int",
                new XAttribute("name", name),
                new XAttribute("value", value));

        private static XElement StringNode(string name, string value)
            => new XElement("string",
                new XAttribute("name", name),
                new XAttribute("value", value ?? ""),
                new XAttribute("wide", "true"));

        private static XElement IntMember(string name, int value, int min, int max)
            => new XElement("member",
                new XAttribute("name", name),
                new XElement("int",
                    new XAttribute("name", "Value"),
                    new XAttribute("value", value)),
                new XElement("int",
                    new XAttribute("name", "Min"),
                    new XAttribute("value", min)),
                new XElement("int",
                    new XAttribute("name", "Max"),
                    new XAttribute("value", max)));

        private static XElement StringMember(string name, string value)
            => new XElement("member",
                new XAttribute("name", name),
                StringNode("s", value));

        private static int GetInt(XElement parent, string name)
        {
            var n = parent.Elements("int")
                .FirstOrDefault(x => Attr(x, "name") == name);

            return int.TryParse(Attr(n, "value"), out var v) ? v : 0;
        }

        private static string GetString(XElement parent, string name)
        {
            var n = parent.Elements("string")
                .FirstOrDefault(x => Attr(x, "name") == name);

            return Attr(n, "value");
        }

        private static int GetNestedValue(XElement parent, string memberName)
        {
            var m = parent.Elements("member")
                .FirstOrDefault(x => Attr(x, "name") == memberName);

            return m == null ? 0 : GetInt(m, "Value");
        }

        private static string GetNestedString(XElement parent, string memberName)
        {
            var m = parent.Elements("member")
                .FirstOrDefault(x => Attr(x, "name") == memberName);

            return m == null ? "" : GetString(m, "s");
        }

        private static string Attr(XElement? e, string name)
            => e?.Attribute(name)?.Value ?? "";

        private static string NormalizeHex(string s)
            => new string(s.Where(Uri.IsHexDigit).ToArray());

        private static string FormatHex(string hex)
        {
            hex = NormalizeHex(hex);
            return Environment.NewLine + "    " +
                   string.Join(Environment.NewLine + "    ",
                       Enumerable.Range(0, (hex.Length + 63) / 64)
                           .Select(i => hex.Substring(i * 64,
                               Math.Min(64, hex.Length - i * 64))))
                   + Environment.NewLine;
        }
    }

    // ------------------------------------------------------------
    // Generic object node for Actions / Filter
    // ------------------------------------------------------------
    public class PleObject
    {
        public string Class { get; set; } = "";
        public string Id { get; set; } = "";
        public XElement Raw { get; set; } = new XElement("obj");

        public static PleObject FromXElement(XElement e)
        {
            return new PleObject
            {
                Class = e.Attribute("class")?.Value ?? "",
                Id = e.Attribute("ID")?.Value ?? "",
                Raw = new XElement(e)
            };
        }

        public XElement ToXElement() => new XElement(Raw);
    }
}