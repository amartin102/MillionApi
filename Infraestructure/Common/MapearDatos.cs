using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Infraestructure.Documents;
using MongoDB.Bson;

namespace Infraestructure.Common
{
    public class MapearDatos
    {
        public static Property MapToEntity(PropertyDocument dto)
        {
            return new Property
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                CodeInternal = dto.CodeInternal,
                Year = dto.Year,
                CreationDate = new DateConverter().ConvertUtcToLocal(DateTime.UtcNow).ToString(), // Fixed: Changed to DateTime
                UpdateDate = null,
                Owner = new Owner
                {
                    IdOwner = dto.Owner.IdOwner,
                    Name = dto.Owner.Name,
                    Address = dto.Owner.Address,
                    Photo = dto.Owner.Photo,
                    Birthday = dto.Owner.Birthday
                },
                Images = dto.Images.Select(i => new PropertyImage
                {
                    IdPropertyImage = i.IdPropertyImage,
                    File = i.File,
                    Enabled = i.Enabled
                }).ToList(),
                Traces = dto.Traces.Select(t => new PropertyTrace
                {
                    IdPropertyTrace = t.IdPropertyTrace,
                    DateSale = t.DateSale,
                    Name = t.Name,
                    Value = t.Value,
                    Tax = t.Tax
                }).ToList()
            };
        }

        public static PropertyDocument ToDocument(Property entity)
        {
            return new PropertyDocument
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = entity.Name,
                Address = entity.Address,
                Price = entity.Price,
                CodeInternal = entity.CodeInternal,
                Year = entity.Year,
                CreationDate = new DateConverter().ConvertUtcToLocal(DateTime.UtcNow), // Fixed: Changed to DateTime
                UpdateDate = null,
                Owner = new OwnerDocument
                {
                    IdOwner = entity.Owner.IdOwner,
                    Name = entity.Owner.Name,
                    Address = entity.Owner.Address,
                    Photo = entity.Owner.Photo,
                    Birthday = (entity.Owner.Birthday != null ? (DateTime)entity.Owner.Birthday : null)
                },
                Images = entity.Images.Select(i => new PropertyImageDocument
                {
                    IdPropertyImage = i.IdPropertyImage,
                    File = i.File,
                    Enabled = i.Enabled
                }).Where(x=> x.File != null).ToList(),
                Traces = entity.Traces.Select(t => new PropertyTraceDocument
                {
                    IdPropertyTrace = t.IdPropertyTrace,
                    DateSale = (t.DateSale != null ? (DateTime)t.DateSale : null),
                    Name = t.Name,
                    Value = (decimal)t.Value,
                    Tax = (decimal)t.Tax
                }).Where(x=> x.Value != 0).ToList()
            };
        }


    }

}
