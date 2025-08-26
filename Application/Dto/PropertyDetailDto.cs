using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class PropertyDetailDto
    {
        public string Id { get; set; } = string.Empty;      // Id de la propiedad
        public string IdOwner { get; set; } = string.Empty; // Id del dueño
        public string Name { get; set; } = string.Empty;    // Nombre de la propiedad
        public string Address { get; set; } = string.Empty; // Dirección de la propiedad
        public decimal Price { get; set; }                  // Precio
        public string Image { get; set; } = string.Empty;   // Imagen de la propiedad (URL o base64)
        public int Year { get; set; }
        public string OwnerName { get; set; } //Nombre del propietario

    }
}
