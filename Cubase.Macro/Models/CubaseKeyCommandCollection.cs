using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubase.Macro.Models
{
    public class CubaseKeyCommandCollection : List<CubaseKeyCommand>    
    {
        public List<CubaseKeyCommand> GetAllocated()
        {
            return this.Where(c => !string.IsNullOrWhiteSpace(c.Key)).ToList(); 
        }

        public List<string> GetCategories()
        {
            return this.Select(c => c.Category).Distinct().OrderBy(c => c).ToList();
        }
        
        public List<string> GetKeys()
        {
            return this.Select(c => c.Key).Where(k => !string.IsNullOrWhiteSpace(k)).Distinct().OrderBy(k => k).ToList();
        }

        public CubaseKeyCommand GetByCategoryAndName(string category, string name)
        {
            return this.FirstOrDefault(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<CubaseKeyCommand> GetByCategory(string category)
        {
            return this.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).OrderBy(c => c.Name).ToList();
        }

        public List<CubaseKeyCommand> GetByKey(string key)
        {
            return this.Where(x => x.Key.Contains(key, StringComparison.OrdinalIgnoreCase)).ToList();   
        }


        public List<CubaseKeyCommand> GetByName(string name)
        {
            return this.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).OrderBy(c => c.Name).ToList();
        }

    }


    public class CubaseKeyCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Category {  get; set; } = string.Empty;


        public static CubaseKeyCommand Create()
        {
            return new CubaseKeyCommand();
        }
    }
}
