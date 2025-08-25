using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PropertyImage
    {
        public required string IdPropertyImage { get; set; }
        public required string File { get; set; }
        public required bool Enabled { get; set; }
    }
}
