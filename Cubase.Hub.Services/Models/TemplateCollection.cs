namespace Cubase.Hub.Services.Models
{
    public class TemplateCollection : List<Template>
    {
    }

    public class Template
    {
        public string TemplateName { get; set; }

        public string TemplateLocation { get; set; }
    }
}
