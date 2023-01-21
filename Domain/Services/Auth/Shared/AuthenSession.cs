namespace Domain.Services.Auth.Shared;

public class AuthenSession
{
    public string CodeChallenge { get; set; } = String.Empty;
    public string UserId { get; set; } = String.Empty;
}
