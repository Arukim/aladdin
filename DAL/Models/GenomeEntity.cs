using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Aladdin.DAL.Models
{
    public class GenomeEntity
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}