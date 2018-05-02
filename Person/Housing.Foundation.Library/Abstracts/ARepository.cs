using Housing.Foundation.Library.Interfaces;
using Housing.Foundation.Library.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Housing.Foundation.Library.Abstracts
{
    /// <summary>
    /// This abstract class implements IRepository interface.
    /// This will connect to MongoDB database using Constructor Dependency Injection.
    /// </summary>
    /// <typeparam name="TModel">Can be Person, Batch or Apartment</typeparam>
    public abstract class ARepository<TModel> : IRepository<TModel> where TModel : IModel
    {
        public const string MongoDbIdName = "_id";

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _db;

        protected readonly IMongoCollection<TModel> _collection;

        protected readonly TimeSpan CacheExpiration;

        // SalesForceConfig contains all necessary information to connect to Salesforce API and retrieve data.
        protected readonly Dictionary<string, string> SalesforceConfig;

        // SalesForceURLs contains all the URLs required for all HTTP requests
        protected readonly Dictionary<string, string> SalesforceURLs;

        // Thread-safe lock.
        private object lockObj = new object();

        /// <summary>
        /// DI with settings class, connects to MongoDB.
        /// </summary>
        /// <param name="settings"></param>
        public ARepository(IOptions<Settings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            if (_client != null)
            {
                _db = _client.GetDatabase(settings.Value.Database);
                _collection = _db.GetCollection<TModel>(settings.Value.CollectionName);
            }
            CacheExpiration = new TimeSpan(0, settings.Value.CacheExpirationMinutes, 0);

            SalesforceConfig = new Dictionary<string, string>();
            SalesforceURLs = new Dictionary<string, string>();

            // Group the salesforce request key-value pairs into dictionary
            SalesforceConfig.Add("grant_type", "password");
            SalesforceConfig.Add("client_id", settings.Value.ClientId);
            SalesforceConfig.Add("client_secret", settings.Value.ClientSecret);
            SalesforceConfig.Add("username", settings.Value.Username);
            SalesforceConfig.Add("password", settings.Value.Password);

            // Group the URLs together
            SalesforceURLs.Add("base", settings.Value.BaseURL);
            SalesforceURLs.Add("login", settings.Value.LoginURLExtension);
            SalesforceURLs.Add("resource_base", settings.Value.ResourceBaseExtension);

            // Update the salesforce database asynchronously only once, during
            // the startup of the program.
            // TODO: Automate this retrieval process, every 24 hours for example.
            Task.Run(() => RetrieveFromDataSource()).Wait();
        }

        /// <summary>
        /// This will retrieve object from a data source, copy into mongoDB and return new object from mongoDB
        /// </summary>
        /// <param name="model">Can be any of Person, Apartment, Batch</param>
        /// <returns>Returns a model from MongoDB</returns>
        private async Task RetrieveFromDataSource()
        {

            // Client used for login
            HttpClient authClient = new HttpClient();

            // Content contains all the key-value pairs needed in the request body
            HttpContent content = new FormUrlEncodedContent(SalesforceConfig);

            // Build the URL for login by concatenating the base URL for Salesforce and additional url for login purposes
            string loginURL = SalesforceURLs["base"] + SalesforceURLs["login"];

            // POST to the login URL using the Content as the request body
            HttpResponseMessage message =
                await authClient.PostAsync(loginURL, content);

            string responseString = await message.Content.ReadAsStringAsync();

            // Request body for login will contain the access_token
            JObject obj = JObject.Parse(responseString);
            string oauthToken = (string)obj["access_token"];

            // Client used to GET the data from Salesforce
            HttpClient queryClient = new HttpClient();

            // Build the url using the URLs and URL extensions from appsettings.json
            string restQuery = SalesforceURLs["base"] + SalesforceURLs["resource_base"];

            // Define headers for the GET request
            // Authorization header and Accept json header
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, restQuery);
            req.Headers.Add("Authorization", "Bearer " + oauthToken);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await queryClient.SendAsync(req);

            // GET request returns a list of contacts from Salesforce
            // Parse the response into a JSON object.
            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(jsonResponse);

            // Take the recent items from the json response
            JArray jsonRecentItems = JArray.Parse(jsonObject["recentItems"].ToString());

            // Returns an IEnum of all contacts converted to object models
            IEnumerable<TModel> allContacts = await GetAllContacts(jsonRecentItems, oauthToken);

            // To ensure thread safety, keep a lock object on the critical section of
            // updating the mongoDB.
            lock (lockObj)
            {
                UpdateMongoDB(allContacts);
            }

        }

        /// <summary>
        /// Get all contacts (persons) from the mongoDB.
        /// </summary>
        /// <param name="contacts">The object models that are composed from the Salesforce API.</param>
        public void GetMongoDBContacts(IEnumerable<TModel> dataContacts)
        {
            // Get all contacts by passing an always true predicate into a list.
            var contacts = _collection.Find(_ => true).ToList();
            foreach (var contact in contacts)
            {
                Console.WriteLine(contact.ToString());
            }

        }

        /// <summary>
        /// For every request from the Salesforce API, update the mongoDB if there are contacts
        /// missing from that particular database.
        /// 
        /// Afterwards, check if mongoDB has contacts that are not in the salesforce API data. If so, delete
        /// those particular contacts.
        /// </summary>
        /// <param name="dataContacts">The object models that are composed from the Salesforce API.</param>
        public void UpdateMongoDB(IEnumerable<TModel> dataContacts)
        {
            // Get the contacts in the Person collection, check for existing contacts.
            // If not present, add to collection.
            var mongoContacts = _collection.Find(_ => true).ToList();
            foreach (TModel dataContact in dataContacts)
            {

                var existingContact = mongoContacts.Find(item => dataContact.ModelId == item.ModelId);

                if (existingContact == null)
                {
                    _collection.InsertOne(dataContact);
                }
            }

            // Next, if the contacts in the MongoDB does not exist in the salesforce API data, then
            // remove it from the MongoDB.
            List<TModel> dataContactList = new List<TModel>(dataContacts);
            foreach (var mongoContact in mongoContacts)
            {
                var existingContact = dataContactList.Find(item => mongoContact.ModelId == item.ModelId);
                if (existingContact == null)
                {
                    _collection.DeleteMany(Builders<TModel>.Filter.Eq("_id", new ObjectId(mongoContact.ModelId)));
                }

            }
        }


        /// <summary>
        /// Performs the mapping from data source's JSON object to TModel object model
        /// </summary>
        /// <param name="jsonObject">JSON object from a data source, e.g. Salesforce</param>
        /// <returns>TModel object model with fields mapped from JSON</returns>
        protected abstract TModel MapJsonToModel(JObject jsonObject);

        /// <summary>
        /// This takes the JsonArray from the recentItems key from the JSON response
        /// Each recentItem has a url key that points to each contact located under the attributes
        ///
        /// CAUTION: This is tightly coupled to Salesforce
        /// </summary>
        private async Task<IEnumerable<TModel>> GetAllContacts(JArray contactList, string authToken)
        {
            HttpClient queryClient = new HttpClient();
            List<TModel> modelList = new List<TModel>();

            // Iterate through each contact in the JSON array
            // Each contact contains a URL pointing to its resource
            // The URL can be accessed through the url property of the attributes property within the contact
            foreach (var contact in contactList)
            {
                // Setup the base request for GET Http Request for each Contact
                // All requests need the Authorization header and all accepts json
                HttpRequestMessage contactRequest = new HttpRequestMessage();
                contactRequest.Method = HttpMethod.Get;
                contactRequest.Headers.Add("Authorization", "Bearer " + authToken);
                contactRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build each contact url using the url found in the attributes property
                string contactURL = SalesforceURLs["base"] + contact["attributes"]["url"];
                contactRequest.RequestUri = new Uri(contactURL);

                // Send the request to the specific contact
                HttpResponseMessage response = await queryClient.SendAsync(contactRequest);
                string jsonContactAsString = await response.Content.ReadAsStringAsync();

                // Receive the contact as a JSON object and map it to object model
                // Add the mapped object to the list of mapped models
                JObject jsonContact = JObject.Parse(jsonContactAsString);
                modelList.Add(MapJsonToModel(jsonContact));
            }

            return modelList;
        }

        /// <summary>
        /// This will check if cache is expired or not against database lastModifiedDate, This is used with GetById function.
        /// </summary>
        /// <param name="model">Can be any of Person, Apartment, Batch</param>
        /// <returns>True if cache is still valid, false if otherwise</returns>
        private bool IsCacheEntryExpired(TModel model)
        {
            TimeSpan lastModifiedInterval = DateTime.Now - model.LastModified;
            if (lastModifiedInterval > CacheExpiration) return true;
            return false;
        }

        /// <summary>
        /// This function will return all the documents as list from MongoDB as BsonDocument.
        /// </summary>
        /// <returns>List of documents.</returns>
        public async Task<IEnumerable<TModel>> GetAll()
        {
            // Update the mongoDB with salesforce API.
            // TODO: Abstract data source.

            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <summary>
        /// This function will return a document from MongoDB based on ModelId
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <returns>A document from MongoDB</returns>
        public async Task<TModel> GetById(string id)
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

            FilterDefinition<TModel> filter = Builders<TModel>.Filter.Eq(MongoDbIdName, theObjectId);

            TModel result = await _collection.Find(filter).FirstAsync();

            // TODO: Figure out caching for single item.
            
            return result;
        }

        /// <summary>
        /// This will perform POST method, insert one document to database
        /// </summary>
        /// <param name="model">Can be any of Person, Apartment, or Batch</param>
        public async Task Create(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.ModelId = null;
            model.LastModified = DateTime.Now;

            await _collection.InsertOneAsync(model);
        }

        /// <summary>
        /// This will update a database based on ModelId, only if id is correct.
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <param name="model">Model can be any of Person, Batch, or Apartment.</param>
        /// <returns>True if updated with given model, false otherwise</returns>
        public async Task<bool> UpdateById(string id, TModel model)
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
            FilterDefinition<TModel> filter = Builders<TModel>.Filter.Eq(MongoDbIdName, theObjectId);

            ReplaceOneResult result = await _collection.ReplaceOneAsync(filter, model);

            return (result.IsAcknowledged && result.ModifiedCount == 1);
        }

        /// <summary>
        /// This will delete a document from collection based on ModelId.
        /// </summary>
        /// <param name="id">ModelId</param>
        /// <returns>True if deleted, false otherwise</returns>
        public async Task<bool> DeleteById(string id)
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

            //filter based on given model
            FilterDefinition<TModel> filter = Builders<TModel>.Filter.Eq(MongoDbIdName, theObjectId);

            DeleteResult result = await _collection.DeleteOneAsync(filter);

            return (result.IsAcknowledged && result.DeletedCount == 1);
        }
    }
}