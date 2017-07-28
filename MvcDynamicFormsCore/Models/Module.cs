using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDynamicFormsCore.Models
{
    public class Module
    {
        public Module()
        {
            Sections = new List<Section>();
        }

        public string Name { get; set; }
        public List<Section> Sections { get; set; }
    }
}
