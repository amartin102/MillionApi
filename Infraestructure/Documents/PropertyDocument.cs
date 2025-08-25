using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infraestructure.Documents
{
    public class PropertyDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public OwnerDocument Owner { get; set; } = new();
        public List<PropertyImageDocument> Images { get; set; } = new();
        public List<PropertyTraceDocument> Traces { get; set; } = new();
    }
}
