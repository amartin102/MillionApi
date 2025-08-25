using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;

namespace Infraestructure.Interface
{
    public interface IPropertyRepository
    {
        Task <bool> CreateAsync(Property property, CancellationToken ct);
        Task<Property?> GetByIdAsync(string id, CancellationToken ct);

        Task<(IEnumerable<Property> Properties, int TotalCount)> GetFilteredAsync(PropertyFilterDto filter, CancellationToken ct);


        //Task<IEnumerable<Property>> GetAllAsync(CancellationToken ct);
        //Task<bool> UpdateAsync(Property property, CancellationToken ct);
        //Task<bool> DeleteAsync(string id, CancellationToken ct);
    }
}
