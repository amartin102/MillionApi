using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Documents
{
    public class PropertyTraceDocument
    {
        public string IdPropertyTrace { get; set; } = string.Empty;
        public DateTime? DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}
