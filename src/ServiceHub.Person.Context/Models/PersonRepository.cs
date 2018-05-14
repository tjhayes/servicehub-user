using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
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
        
        private readonly HttpClient _salesforceapi;

        private readonly string _baseUrl;

        private readonly string _getAll; 

        private readonly MetaData _metadata;

        private readonly string _MetaDataCollection;

        public PersonRepository(IOptions<ServiceHub.Person.Library.Models.Settings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _salesforceapi = new HttpClient();
            _baseUrl = settings.Value.BaseURL;
            _getAll =  settings.Value.GetAll;
            _MetaDataCollection = settings.Value.MetaDataCollectionName;
            if (_client != null)
            {
                _db = _client.GetDatabase(settings.Value.Database);
                _collection = _db.GetCollection<Person>(settings.Value.CollectionName);

                // Obtaining metadata
                _metadata = _db.GetCollection<MetaData>(settings.Value.MetaDataCollectionName)
                                .Find(p=> p.ModelId == settings.Value.MetaDataId).FirstOrDefault();
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

        private async Task<List<Person>> ReadFromSalesForce()
        {
            var result = await _salesforceapi.GetAsync( _baseUrl + _getAll);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<Person> personlist = null;

                if  (content != null  ){
               
                personlist = JsonConvert.DeserializeObject<List<Person>>(content);
                }
                return personlist;

            }
            else
                return null;
        }

          public void UpdateMongoDB(List<Person> personlist)
        {
            // Get the contacts in the Person collection, check for existing contacts.
            // If not present, add to collection.
            var mongoContacts = _collection.Find(_ => true).ToList();
            foreach (var person in personlist)
            {

                var existingContact = mongoContacts.Find(item => person.ModelId == item.ModelId);

                if (existingContact == null)
                {
                    _collection.InsertOne(person);
                }
            }

            foreach (var mongoContact in mongoContacts)
            {
                var existingContact = personlist.Find(item => mongoContact.ModelId == item.ModelId);
                if (existingContact == null)
                {
                    _collection.DeleteMany(Builders<Person>.Filter.Eq("_id", new ObjectId(mongoContact.ModelId)));
                }

            }
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
