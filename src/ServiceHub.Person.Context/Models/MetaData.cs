using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ServiceHub.Person.Context.Models{

  public class MetaData{


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string ModelId { get; set; }
        public DateTime LastModified {get;set;}

        public long Count {get; set;}


  } 
}