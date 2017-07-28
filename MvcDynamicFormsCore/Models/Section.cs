using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDynamicFormsCore.Models
{
    public class Section
    {
        public Section() {
            Questions = new List<Question>();
        }

        public string Name { get; set; }
        public List<Question> Questions { get; set; }
    }
}
