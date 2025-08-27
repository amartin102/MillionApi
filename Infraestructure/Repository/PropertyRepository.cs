using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Infraestructure.MongoDb;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Infraestructure.Documents;
using Infraestructure.Common;
using Infraestructure.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Infraestructure.Repository
{
    [ExcludeFromCodeCoverage]
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<PropertyDocument> _col;

        public PropertyRepository(IOptions<MongoSettings> opt)
        {
            var client = new MongoClient(opt.Value.ConnectionString);
            var db = client.GetDatabase(opt.Value.DatabaseName);
            _col = db.GetCollection<PropertyDocument>(opt.Value.CollectionName);

            // Índices (idempotentes)
            var idx = new List<CreateIndexModel<PropertyDocument>>
        {
            new(Builders<PropertyDocument>.IndexKeys.Ascending(x => x.Price)),
            new(Builders<PropertyDocument>.IndexKeys.Text(x => x.Name).Text(x => x.Address))
        };
            _col.Indexes.CreateMany(idx);
        }
               
        public async Task<bool> CreateAsync(PropertyItemCreateDto dto, CancellationToken ct)
        {
            var doc = MapearDatos.ToDocument(dto);  
            await _col.InsertOneAsync(doc, cancellationToken: ct);
            return true;
        }

        public async Task<PropertyDetailDto?> GetByIdAsync(string id, CancellationToken ct)
        {
            var doc = await _col.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
            return doc is null ? null : MapearDatos.MapToDetail(doc);
        }

       public async Task<(IEnumerable<PropertyItemDto> Properties, int TotalCount)>GetFilteredAsync(PropertyFilterDto filter, CancellationToken ct)
        {
            var builder = Builders<PropertyDocument>.Filter;
            var filters = new List<FilterDefinition<PropertyDocument>>();
                        
            if (!string.IsNullOrEmpty(filter.Name))
                filters.Add(builder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));

            if (!string.IsNullOrEmpty(filter.Address))
                filters.Add(builder.Regex(p => p.Address, new BsonRegularExpression(filter.Address, "i")));

            if (filter.MinPrice.HasValue)
                filters.Add(builder.Gte(p => p.Price, filter.MinPrice.Value));

            if (filter.MaxPrice.HasValue)
                filters.Add(builder.Lte(p => p.Price, filter.MaxPrice.Value));
           
            var combinedFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;

            var totalCount = await _col.CountDocumentsAsync(combinedFilter, cancellationToken: ct);

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var docs = await _col.Find(combinedFilter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(ct);

            return (docs.Select(MapearDatos.MapToItemFilter), (int)totalCount);
        }

    }
}
