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

namespace Infraestructure.Repository
{
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
               
        public async Task<bool> CreateAsync(Property property, CancellationToken ct)
        {
            var doc = MapearDatos.ToDocument(property);  
            await _col.InsertOneAsync(doc, cancellationToken: ct);
            return true;
        }

        public async Task<Property?> GetByIdAsync(string id, CancellationToken ct)
        {
            var doc = await _col.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
            return doc is null ? null : MapearDatos.MapToEntity(doc);
        }

        public async Task<(IEnumerable<Property>, int)> GetFilteredAsync(PropertyFilterDto filter, CancellationToken ct)
        {
            var builder = Builders<PropertyDocument>.Filter;
            var filters = new List<FilterDefinition<PropertyDocument>>();

            if (!String.IsNullOrEmpty(filter.Address)  || !String.IsNullOrEmpty(filter.Name) || filter.MinPrice.HasValue || filter.MaxPrice.HasValue) {
                if (!string.IsNullOrEmpty(filter.Name))
                    filters.Add(builder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));

                if (!string.IsNullOrEmpty(filter.Address))
                    filters.Add(builder.Regex(p => p.Address, new BsonRegularExpression(filter.Address, "i")));

                if (filter.MinPrice.HasValue)
                    filters.Add(builder.Gte(p => p.Price, filter.MinPrice.Value));

                if (filter.MaxPrice.HasValue)
                    filters.Add(builder.Lte(p => p.Price, filter.MaxPrice.Value));
            }
           
            var combinedFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;

            // Total antes de paginar
            var totalCount = await _col.CountDocumentsAsync(combinedFilter, cancellationToken: ct);

            // Valores por defecto de paginación
            var pageNumber = (filter?.PageNumber ?? 1) < 1 ? 1 : filter.PageNumber;
            var pageSize = (filter?.PageSize ?? 10) < 1 ? 10 : filter.PageSize;

            // Query con paginación
            var docs = await _col.Find(combinedFilter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(ct);

            // Mapear a entidades de dominio
            return (docs.Select(MapearDatos.MapToEntity), (int)totalCount);
        }

    }
}
