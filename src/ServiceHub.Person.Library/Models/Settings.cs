using System.Collections.Generic;

namespace ServiceHub.Person.Library.Models
{
    /// <summary>
    /// This class is used to get connectionstring name, database name, and collection name from appSettings.json file for MongoDB.
    /// Then, we can inject this to any dbcontext with IOptions
    /// </summary>
    public class Settings
    {

        //connectionstring name
        public readonly string ConnectionString;
        //database name
        public readonly string Database;
        //collection name
        public readonly string CollectionName;

        public readonly string MetaDataCollectionName;
        public readonly string MetaDataId;

        // Salesforce URLs
        public readonly string BaseURL;
        public readonly string GetById;

        public Settings(List<string> strings)
        {
            ConnectionString = strings[0];
            Database = strings[1];
            CollectionName = strings[2];
            MetaDataCollectionName = strings[3];
            MetaDataId = strings[4];
            BaseURL = strings[5];
            GetById = strings[6];
        }
    }
}
