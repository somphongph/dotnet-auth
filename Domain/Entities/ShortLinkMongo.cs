using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class ShortLinkMongo : BaseMongoEntity
    {
        public string ShortKey { get; set; } = String.Empty;
        public string ShortUrl { get; set; } = String.Empty;
        public string LongUrl { get; set; } = String.Empty;
        public DateTime ExpiredOn { get; set; }
    }
}