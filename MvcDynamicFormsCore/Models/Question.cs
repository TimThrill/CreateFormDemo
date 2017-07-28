using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDynamicFormsCore.Models
{
    public class Question
    {
        public Question()
        {
            Values = new List<object>();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public List<object> Values { get; set; }
        public bool Required { get; set; }
    }
}
