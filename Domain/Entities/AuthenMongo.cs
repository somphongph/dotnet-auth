using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class AuthenMongo : BaseMongoEntity
{
    [BsonElement("userId")]
    public string UserId { get; set; } = String.Empty;

    [BsonElement("refreshToken")]
    public string RefreshToken { get; set; } = String.Empty;

    // TTL
    [BsonDateTimeOptions]
    [BsonElement("expiredOn")]
    public DateTime ExpiredOn { get; set; }
}
