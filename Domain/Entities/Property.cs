using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Property
    {
        public required string Id { get; set; }
        public required string IdOwner { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required string Address { get; set; }
        public required string ImageUrl { get; set; }
        public required string CreationDate { get; set; }
        public string? UpdateDate { get; set; }
    }
}
