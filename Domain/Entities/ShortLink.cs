using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("short_link")]
    public class ShortLink : BaseEntity
    {
        public string ShortKey { get; set; } = String.Empty;
        public string ShortUrl { get; set; } = String.Empty;
        public string LongUrl { get; set; } = String.Empty;
        public DateTime ExpiredOn { get; set; }
    }
}