using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Domain.Interface;

namespace Application.Services
{
    public class PropertyService
    {
       private readonly IPropertyInterface _iproperty;

        public PropertyService(IPropertyInterface propertyInterface) {
            this._iproperty = propertyInterface;
        }

        public async Task<PaginationResult<Property>> SearchAsync(PropertyFilterDto filter, CancellationToken ct)
        {
            return await _iproperty.SearchAsync(filter, ct);
        }
        public async Task<Property?> GetByIdAsync(string id, CancellationToken ct)
        {
            return await _iproperty.GetByIdAsync(id, ct);
        }
        public async Task<string> CreateAsync(PropertyItemDto propertyDto, CancellationToken ct)
        {
            return await _iproperty.CreateAsync(Map.MapDataProperty(propertyDto), ct);
        }
        public async Task<bool> UpdatePropertyAsync(PropertyItemDto propertyDto, CancellationToken ct)
        {
            return await _iproperty.UpdateAsync(Map.MapDataProperty(propertyDto), ct);
        }
        public async Task<bool> DeletePropertyAsync(string id, CancellationToken ct)
        {
            return await _iproperty.DeleteAsync(id, ct);
        }

    }
}
