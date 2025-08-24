using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Domain.Interface;

namespace Domain.Interface
{
    public interface IPropertyInterface
    {
        Task<PaginationResult<Property>> SearchAsync(PropertyFilterDto filter, CancellationToken ct);
        Task<Property?> GetByIdAsync(string id, CancellationToken ct);
        Task<string> CreateAsync(Property property, CancellationToken ct);
        Task<bool> UpdateAsync(Property property, CancellationToken ct);
        Task<bool> DeleteAsync(string id, CancellationToken ct);
    }
}
