using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Aladdin.DAL.Models
{
    public class GameEntity
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("isWinner")]
        public bool IsWinner { get; set; }
        [BsonElement("gameId")]
        public string GameId { get; set; }
        [BsonElement("accountId")]
        public string AccoundId { get; set; }
        [BsonElement("accountName")]
        public string AccountName { get; set; }
        [BsonElement("genomeId")]
        public string GenomeId { get; set; }
    }
}