using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceHub.Person.Context.Models
{
    class Person
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BatchName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public bool HasCar { get; set; }
        [Required]
        public bool IsMale { get; set; }
        [Required]
        public string Address { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ModelId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
