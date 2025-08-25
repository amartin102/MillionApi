using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Interface;

namespace Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _iproperty;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyInterface, IMapper mapper ) {
            this._iproperty = propertyInterface;
            this._mapper = mapper;
        }

        public async Task<PropertyDetailDto?> GetByIdAsync(string id, CancellationToken ct)
        {
            var propertyDetailDto = new PropertyDetailDto();
            var result = await _iproperty.GetByIdAsync(id, ct);

            if (result != null)
            {
                propertyDetailDto.Address = result.Address ?? string.Empty;
                propertyDetailDto.Name = result.Name ?? string.Empty;
                propertyDetailDto.Price = result.Price;
                //propertyDetailDto.Id = result.Id ?? string.Empty;
               // propertyDetailDto.Image = result.Images?.FirstOrDefault()?.File ?? string.Empty;
            }

            return propertyDetailDto;
        }
        public async Task<bool> CreateAsync(PropertyItemCreateDto propertyDto, CancellationToken ct)
        {
            var propertyEntity = _mapper.Map<Property>(propertyDto);
            return await _iproperty.CreateAsync(propertyEntity, ct);
        }

        public async Task<PaginationResult<PropertyItemDto>> GetPropertiesAsync(PropertyFilterDto filter, CancellationToken ct)
        {
            var (properties, totalCount) = await _iproperty.GetFilteredAsync(filter, ct);

            return new PaginationResult<PropertyItemDto>
            {
                Items = properties.Select(_mapper.Map<PropertyItemDto>),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }


        //public async Task<bool> UpdatePropertyAsync(PropertyItemDto propertyDto, CancellationToken ct)
        //{
        //    return await _iproperty.UpdateAsync(Map.MapDataProperty(propertyDto), ct);
        //}
        //public async Task<bool> DeletePropertyAsync(string id, CancellationToken ct)
        //{
        //    return await _iproperty.DeleteAsync(id, ct);
        //}

    }
}
