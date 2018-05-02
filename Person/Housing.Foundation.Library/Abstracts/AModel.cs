using System;
using System.Collections.Generic;
using System.Text;
using Housing.Foundation.Library.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Housing.Foundation.Library.Abstracts
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
