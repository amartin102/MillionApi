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
            var result = await _iproperty.GetByIdAsync(id, ct);
            return result;
        }
        public async Task<bool> CreateAsync(PropertyItemCreateDto propertyDto, CancellationToken ct)
        {            
            return await _iproperty.CreateAsync(propertyDto, ct);
        }

        public async Task<PaginationResult<PropertyItemDto>> GetAllPropertiesByFilter(PropertyFilterDto filter, CancellationToken ct)
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


    }
}
