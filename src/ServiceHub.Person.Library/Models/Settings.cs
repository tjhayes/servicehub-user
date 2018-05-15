using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace ServiceHub.Person.Library.Models
{
    /// <summary>
    /// This class is used to get connectionstring name, database name, and collection name from appSettings.json file for MongoDB.
    /// Then, we can inject this to any dbcontext with IOptions
    /// </summary>
    public static class Settings
    {

        //connectionstring name
        public static readonly string ConnectionString;
        //database name
        public static readonly string Database;
        //collection name
        public static readonly string CollectionName;

        public static readonly string MetaDataCollectionName;
        public static readonly string MetaDataId;

        // Salesforce URLs
        public static readonly string BaseURL;
        public static readonly string GetById;
        public static IConfiguration Configuration { get; }
        public static IHostingEnvironment Env { get; }

        static Settings()
        {
            if (Env.IsStaging())
            {
                ConnectionString = Environment.GetEnvironmentVariable("MongoDB:ConnectionString");
                Database = Environment.GetEnvironmentVariable("MongoDB:Database");
                CollectionName = Environment.GetEnvironmentVariable("MongoDB:Collection");
                MetaDataCollectionName = Environment.GetEnvironmentVariable("MongoDB:MetaDataCollection");
                MetaDataId = Environment.GetEnvironmentVariable("MongoDB:MetaDataId");
                BaseURL = Environment.GetEnvironmentVariable("SalesforceURLs:Base");
                GetById = Environment.GetEnvironmentVariable("SalesforceURLs:GetById");
            }
            else
            {
                ConnectionString = Configuration.GetSection("MongoDB:ConnectionString").Value;
                Database = Configuration.GetSection("MongoDB:Database").Value;
                CollectionName = Configuration.GetSection("MongoDB:Collection").Value;
                MetaDataCollectionName = Configuration.GetSection("MongoDB:MetaDataCollection").Value; ;
                MetaDataId = Configuration.GetSection("MongoDB:MetaDataId").Value; ;
                BaseURL = Configuration.GetSection("SalesforceURLs:Base").Value;
                GetById = Configuration.GetSection("SalesforceURLs:GetById").Value;
            }
        }
    }
}
