using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.Person.Library.Models
{
    /// <summary>
    /// This class is used to get connectionstring name, database name, and collection name from appSettings.json file for MongoDB.
    /// Then, we can inject this to any dbcontext with IOptions
    /// </summary>
    public class Settings
    {

        //connectionstring name
        public string ConnectionString { get; set; }
        //database name
        public string Database { get; set; }
        //collection name
        public string CollectionName { get; set; }

        public string MetaDataCollectionName { get; set; }
        public string MetaDataId {get; set;}

        public int CacheExpirationMinutes { get; set; }

        // Salesforce request body information
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        // Salesforce URLs
        public string BaseURL { get; set; }
        public string LoginURLExtension { get; set; }
        public string ResourceBaseExtension { get; set; }

        public string GetAll {get; set;}
    }
}
