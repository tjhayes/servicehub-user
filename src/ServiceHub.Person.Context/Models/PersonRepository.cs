using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ServiceHub.Person.Context.Models;
using System.Net.Http.Headers;
using ServiceHub.Person.Context.Interfaces;

namespace ServiceHub.Person.Context.Models 
{
    public class PersonRepository : IRepository<Person>
    {
        public const string MongoDbIdName = "_id";

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _db;

        private readonly IMongoCollection<Person> _collection;

        protected readonly TimeSpan CacheExpiration;

        public PersonRepository(IOptions<ServiceHub.Person.Library.Models.Settings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            if (_client != null)
            {
                _db = _client.GetDatabase(settings.Value.Database);
                _collection = _db.GetCollection<Person>(settings.Value.CollectionName);
            }
            CacheExpiration = new TimeSpan(0, settings.Value.CacheExpirationMinutes, 0);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Person> GetById(string id)
        {
            ObjectId theObjectId;
            try
            {
                theObjectId = new ObjectId(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid ID", ex);
            }

            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq(MongoDbIdName, theObjectId);

            Person result = await _collection.Find(filter).FirstAsync();

            // TODO: Figure out caching for single item.

            return result;
        }

        public Task Create(Person model)
        {
            return Task.Run(() => Console.WriteLine("Not Implemented"));
//            throw new NotImplementedException();
        }

        public Task<bool> UpdateById(string id, Person model)
        {
            return Task.Run(() => false);
//            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(string id)
        {
            return Task.Run(() => false);
//            throw new NotImplementedException();
        }
    }
}
