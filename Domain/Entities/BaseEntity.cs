using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Status { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; } = null;
        public DateTime UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; } = null;
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}