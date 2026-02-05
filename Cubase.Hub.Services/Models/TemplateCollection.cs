using System;
using System.Collections.Generic;
using System.Text;

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
