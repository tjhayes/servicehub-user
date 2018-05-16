using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using ServiceHub.Person.Context.Interfaces;
using System.Data;
using System.Security.Authentication;
using ServiceHub.Person.Library.Models;

namespace ServiceHub.Person.Context.Models 
{
    public class PersonRepository : IRepository<Person>
    {
        public const string MongoDbIdName = "_id";

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _db;

        private readonly IMongoCollection<Person> _collection;
        
        private readonly HttpClient _salesforceapi;

        private readonly string _baseUrl;

        private readonly MetaData _metadata;

        private readonly string _MetaDataCollection;
        private readonly string _metadataId;

        public PersonRepository(Settings Settings)
        {
            MongoClientSettings Msettings = MongoClientSettings.FromUrl( new MongoUrl(Settings.ConnectionString) );
            Msettings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 }; 
            _client = new MongoClient(Msettings);
            //_client = new MongoClient(Settings.ConnectionString); //mayZ have to switch to this
            _salesforceapi = new HttpClient();
            _baseUrl = Settings.BaseURL;
            _MetaDataCollection = Settings.MetaDataCollectionName;
            _metadataId = Settings.MetaDataId;
            if (_client != null)
            {
                _db = _client.GetDatabase(Settings.Database);
                _collection = _db.GetCollection<Person>(Settings.CollectionName);

                // Obtaining metadata
                _metadata = _db.GetCollection<MetaData>(Settings.MetaDataCollectionName)
                                .Find(p=> p.ModelId == Settings.MetaDataId).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Person>> GetAll()
        { 
            var stuff = await _collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine(stuff.ToJson().ToString());
            return stuff;
        }

        public async Task<Person> GetById(string id)
        {
            long newId;
            try
            {
                newId = Convert.ToInt64(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid ID", ex);
            }
            Person result = await _collection.Find(p => p.PersonId == newId).FirstAsync();
            return result;
        }

        private async Task<List<Person>> ReadFromSalesForce()
        {
            
            try
            {
                HttpResponseMessage result = await _salesforceapi.GetAsync( _baseUrl);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    List<Person> personlist = null;

                    if  (content != null  )
                    {               
                        personlist = JsonConvert.DeserializeObject<List<Person>>(content);
                    }
                    return personlist;
                }
                else
                {
                    return new List<Person>();
                }
            }
            catch(Exception e)
            {
                    return new List<Person>();                
            }

        }

        public void UpdateMongoDB(List<Person> personlist)
        {
            if(personlist.Count==0)
                return;
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
        
        public async Task Create(Person model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            model.ModelId = null;
            model.LastModified = DateTime.UtcNow;
            await _collection.InsertOneAsync(model);
        }

        public async Task<bool> UpdateById(string id, Person model)
        {
            ObjectId theObjectId;
            try
            {
                theObjectId = new ObjectId(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid ID", nameof(id), ex);
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            model.ModelId = id;
            model.LastModified = DateTime.Now;
            //find a document which contains the passed model.
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq(MongoDbIdName, theObjectId);
            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, model);
            return (result.IsAcknowledged && result.ModifiedCount == 1);            
        }

        public async Task<bool> DeleteById(string id)
        {
            long newId;
            try
            {
                newId = Convert.ToInt64(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid ID", ex);
            }
            DeleteResult result = await _collection.DeleteOneAsync(p => p.PersonId == newId);
            return (result.IsAcknowledged && result.DeletedCount == 1);
        }

        public void UpdateRepository()
        {
            var updateList = this.ReadFromSalesForce().GetAwaiter().GetResult(); 
            if(updateList.Count != 0)
            {
                this.UpdateMongoDB(updateList);
                var theObjectId = new ObjectId(_metadataId);
                _metadata.LastModified = DateTime.Now;
                //find a document which contains the passed model.
                FilterDefinition<MetaData> filter = Builders<MetaData>.Filter.Eq(MongoDbIdName, theObjectId);
                ReplaceOneResult result = _db.GetCollection<MetaData>(_MetaDataCollection)
                                            .ReplaceOne(filter, _metadata);
                if(!(result.IsAcknowledged && result.ModifiedCount == 1))
                {
                    throw new DBConcurrencyException("Global time not updated.");
                }                                
            }
        }
        public DateTime LastGlobalUpdateTime()
        {
            return _metadata.LastModified;
        }
    }
}