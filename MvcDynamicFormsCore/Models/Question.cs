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
            Values = new List<string>();
            Answers = new List<string>();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public List<string> Answers { get; set; }
        public List<string> Values { get; set; }
        public bool Required { get; set; }
    }
}
