using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Aladdin.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class AccountEntity
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("token")]
        public string Token { get; set; }
        [BsonElement("isBase")]
        public bool IsBase {get;set;}
    }
}