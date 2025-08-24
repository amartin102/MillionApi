using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dto;
using Domain.Entities;
using Domain.Interface;
using Infraestructure.MongoDb;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Infraestructure.Repository
{
    public class PropertyRepository : IPropertyInterface
    {
        private readonly IMongoCollection<PropertyDoc> _col;

        public PropertyRepository(IOptions<MongoSettings> opt)
        {
            var client = new MongoClient(opt.Value.ConnectionString);
            var db = client.GetDatabase(opt.Value.Database);
            _col = db.GetCollection<PropertyDoc>(opt.Value.Collection);

            // Índices (idempotentes)
            var idx = new List<CreateIndexModel<PropertyDoc>>
        {
            new(Builders<PropertyDoc>.IndexKeys.Ascending(x => x.Price)),
            new(Builders<PropertyDoc>.IndexKeys.Text(x => x.Name).Text(x => x.Address))
        };
            _col.Indexes.CreateMany(idx);
        }

        public async Task<PaginationResult<Property>> SearchAsync(PropertyFilterDto f, CancellationToken ct)
        {
            var filter = Builders<PropertyDoc>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(f.Name))
                filter &= Builders<PropertyDoc>.Filter.Regex(x => x.Name, new BsonRegularExpression(f.Name, "i"));

            if (!string.IsNullOrWhiteSpace(f.Address))
                filter &= Builders<PropertyDoc>.Filter.Regex(x => x.Address, new BsonRegularExpression(f.Address, "i"));

            if (f.MinPrice.HasValue)
                filter &= Builders<PropertyDoc>.Filter.Gte(x => x.Price, f.MinPrice.Value);

            if (f.MaxPrice.HasValue)
                filter &= Builders<PropertyDoc>.Filter.Lte(x => x.Price, f.MaxPrice.Value);

            var sort = f.SortBy?.ToLower() switch
            {
                "name" => f.SortDir == "desc"
                    ? Builders<PropertyDoc>.Sort.Descending(x => x.Name)
                    : Builders<PropertyDoc>.Sort.Ascending(x => x.Name),
                _ => f.SortDir == "desc"
                    ? Builders<PropertyDoc>.Sort.Descending(x => x.Price)
                    : Builders<PropertyDoc>.Sort.Ascending(x => x.Price)
            };

            var page = Math.Max(1, f.Page);
            var size = Math.Clamp(f.PageSize, 1, 100);
            var total = await _col.CountDocumentsAsync(filter, cancellationToken: ct);

            var docs = await _col.Find(filter)
                .Sort(sort)
                .Skip((page - 1) * size)
                .Limit(size)
                .ToListAsync(ct);

            return new PaginationResult<Property>(docs.Select(ToEntity).ToList(), page, size, total);
        }

        public async Task<Property?> GetByIdAsync(string id, CancellationToken ct)
        {
            var doc = await _col.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
            return doc is null ? null : ToEntity(doc);
        }

        public async Task<string> CreateAsync(Property property, CancellationToken ct)
        {
            var doc = ToDoc(property);
            doc.Id = ObjectId.GenerateNewId().ToString();
            await _col.InsertOneAsync(doc, cancellationToken: ct);
            return doc.Id;
        }

        public async Task<bool> UpdateAsync(Property property, CancellationToken ct)
        {
            var doc = ToDoc(property);
            var res = await _col.ReplaceOneAsync(x => x.Id == doc.Id, doc, cancellationToken: ct);
            return res.MatchedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken ct)
        {
            var res = await _col.DeleteOneAsync(x => x.Id == id, ct);
            return res.DeletedCount > 0;
        }

        // Documento persistido (evita atributos BSON en dominio)
        private class PropertyDoc
        {
            [BsonId, BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = default!;
            public string IdOwner { get; set; } = default!;
            public string Name { get; set; } = default!;
            public string Address { get; set; } = default!;
            public decimal Price { get; set; }
            public string ImageUrl { get; set; } = default!;
        }

        private static PropertyDoc ToDoc(Property p) => new()
        {
            Id = p.Id,
            IdOwner = p.IdOwner,
            Name = p.Name,
            Address = p.Address,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        };

        private static Property ToEntity(PropertyDoc d) => new()
        {
            Id = d.Id,
            IdOwner = d.IdOwner,
            Name = d.Name,
            Address = d.Address,
            Price = d.Price,
            ImageUrl = d.ImageUrl,
            CreationDate = DateTime.UtcNow.ToString("o"), // ISO 8601 format
        };


    }
}
