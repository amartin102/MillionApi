using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Infraestructure.Documents;
using MongoDB.Bson;

namespace Infraestructure.Common
{
    public class MapearDatos
    {
        public static Property MapToEntity(PropertyDocument item)
        {           
            var property = 
             new Property
            {
                Id = (string.IsNullOrEmpty(item.Id) ? ObjectId.GenerateNewId().ToString() : item.Id),
                Name = item.Name,
                Address = item.Address,
                Price = item.Price,
                CodeInternal = item.CodeInternal,
                Year = item.Year,
                CreationDate = (string.IsNullOrEmpty(item.Id) ? new DateConverter().ConvertUtcToLocal(DateTime.UtcNow).ToString() : item.CreationDate.ToString()), // Fixed: Changed to DateTime
                UpdateDate = null,
                Owner = new Owner
                {
                    IdOwner = item.Owner.IdOwner,
                    Name = item.Owner.Name,
                    Address = item.Owner.Address,
                    Photo = item.Owner.Photo,
                    Birthday = (item.Owner.Birthday != null ? (DateTime)item.Owner.Birthday : null)
                },
                Images = 
                {
                    IdPropertyImage = item.Images.IdPropertyImage,
                    File = item.Images.File,
                    Enabled = item.Images.Enabled
                },
                Traces = new List<PropertyTrace>
                {
                    new PropertyTrace
                    {
                        IdPropertyTrace = item.Traces.IdPropertyTrace,
                        DateSale = (item.Traces.DateSale != null ? (DateTime)item.Traces.DateSale : null),
                        Name = item.Traces.Name,
                        Value = item.Traces.Value,
                        Tax = item.Traces.Tax
                    }
                }
             };

            return property;
        }

        public static PropertyDetailDto MapToDetail(PropertyDocument dto)
        {
            return new PropertyDetailDto
            {
                Id = (string.IsNullOrEmpty(dto.Id) ? ObjectId.GenerateNewId().ToString() : dto.Id),
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                IdOwner = dto.Owner.IdOwner,
                Image = dto.Images?.File ?? string.Empty,
                Year = dto.Year

            };
        }

        public static PropertyItemDto MapToItemFilter(PropertyDocument dto)
        {
            return new PropertyItemDto
            {
                Id = (string.IsNullOrEmpty(dto.Id) ? ObjectId.GenerateNewId().ToString() : dto.Id),
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                IdOwner = dto.Owner.IdOwner,
                Image = dto.Images?.File ?? string.Empty

            };
        }

        public static PropertyDocument ToDocument(PropertyItemCreateDto dto)
        {
            return new PropertyDocument
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                CodeInternal = dto.CodeInternal,
                Year = dto.Year,
                CreationDate = new DateConverter().ConvertUtcToLocal(DateTime.UtcNow), // Fixed: Changed to DateTime
                UpdateDate = null,
                Owner = new OwnerDocument
                {
                    IdOwner = ObjectId.GenerateNewId().ToString(),
                    Name = dto.OwnerName,
                    Address = dto.OwnerAddress,
                    Photo = dto.OwnerPhoto,
                    Birthday = (dto.Birthday != null ? (DateTime)dto.Birthday : null)
                },
                Images = new PropertyImageDocument
                {
                    IdPropertyImage = ObjectId.GenerateNewId().ToString(),
                    File = dto.PropertyFile,
                    Enabled = true
                },
                Traces =  new PropertyTraceDocument
                {
                    IdPropertyTrace = ObjectId.GenerateNewId().ToString(),
                    DateSale = (dto.DateSale != null ? (DateTime)dto.DateSale : null),
                    Name = dto.TraceName ?? string.Empty,
                    Value = (dto.Value != null ? (decimal)dto.Value : 0),
                    Tax = (dto.Tax != null ? (decimal)dto.Tax : 0)
                }
            };
        }


    }

}
