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
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }

        //Owner details
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerAddress { get; set; } = string.Empty;
        public string OwnerPhoto { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }

        //Image details
        public required string PropertyFile { get; set; }

        //Trace details
        public DateTime? DateSale { get; set; }
        public string? TraceName { get; set; }
        public decimal? Value { get; set; }
        public decimal? Tax { get; set; }
    }
}
