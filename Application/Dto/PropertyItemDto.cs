using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Dto
{
    public record PropertyItemDto
    {      
        public string? Id { get; set; }
        public string IdOwner { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Year { get; set; }

    }
}
