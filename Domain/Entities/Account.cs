using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("account")]
public class Account : BaseEntity
{
    public string Name { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string PasswordSalt { get; set; } = String.Empty;
    public string UserRole { get; set; } = String.Empty;
    public DateTime LastSigninOn { get; set; }
}
