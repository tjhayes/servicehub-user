using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Person.Context.Models
{
    public class Person : ServiceHub.Person.Library.Models.Person
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ModelId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
