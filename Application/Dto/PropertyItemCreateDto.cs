using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Dto
{
    public class PropertyItemCreateDto
    {
        public PropertyItemCreateDto()
        {
            Images = new List<PropertyImage>();
            Traces = new List<PropertyTrace>();
        }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public Owner Owner { get; set; }
        public List<PropertyImage> Images { get; set; }
        public List<PropertyTrace> Traces { get; set; }
    }
}
