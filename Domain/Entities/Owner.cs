using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Owner
    {
        public required string IdOwner { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Photo { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
