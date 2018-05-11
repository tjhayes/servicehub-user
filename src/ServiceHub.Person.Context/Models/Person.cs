using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace ServiceHub.Person.Context.Models
{
    public class Person : Library.Models.Person
    {


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        [BsonIgnoreIfDefault]
        public string ModelId { get; set; }

        [JsonIgnore]
        public DateTime LastModified { get; set; }
    }
}
