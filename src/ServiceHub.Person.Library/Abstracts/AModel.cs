using System;
using ServiceHub.Person.Library.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceHub.Person.Library.Abstracts
{
    public abstract class AModel : IModel
    {
        /// <summary>
        /// This is an ObjectId in MongoDB and string when exposing to API
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ModelId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
