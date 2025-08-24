using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;
using Domain.Interface;

namespace Application.Common
{

    public class Map 
    {
        public static Property MapDataProperty(PropertyItemDto propertyDto)
        {
            return new Property
            {
                Id = propertyDto.Id,
                IdOwner = propertyDto.IdOwner,
                Name = propertyDto.Name,
                Price = propertyDto.Price,
                Address = propertyDto.Address,
                ImageUrl = propertyDto.ImageUrl,
                CreationDate = DateTime.UtcNow.ToString("o"), // ISO 8601 format
            };
        }

        public static PropertyItemDto ToDetailDto(Property entity)
        {
            return new PropertyItemDto
            {
                Id = entity.Id,
                IdOwner = entity.IdOwner,
                Name = entity.Name,
                Address = entity.Address,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl
            };
        }

    }

}
