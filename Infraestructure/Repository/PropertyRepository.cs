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
    }
}
