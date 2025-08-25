using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class PropertyImageDto
    {        
        public string IdPropertyImage { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
