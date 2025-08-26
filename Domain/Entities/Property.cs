using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Property
    {
        public Property()
        {
            Traces = new List<PropertyTrace>();
        }

        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }        
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public string CreationDate { get; set; } = string.Empty;
        public string? UpdateDate { get; set; }
        public Owner Owner { get; set; }
        public PropertyImage Images { get; set; }
        public List<PropertyTrace> Traces { get; set; }
    }
}
