using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;

namespace Application.Interface
{
    public interface IPropertyService
    {
        // CREATE
        Task<bool> CreateAsync(PropertyItemCreateDto dto, CancellationToken ct);

        // READ
        Task<PropertyDetailDto?> GetByIdAsync(string id, CancellationToken ct);

        Task<PaginationResult<PropertyItemDto>> GetAllPropertiesByFilter(PropertyFilterDto filter, CancellationToken ct);
       
    }
}
