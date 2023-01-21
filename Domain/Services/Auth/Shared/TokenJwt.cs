namespace Domain.Services.Auth.Shared;

public class TokenJwt
{
    public string TokenType { get; set; } = String.Empty;
    public string AccessToken { get; set; } = String.Empty;
    public string AccessTokenExpire { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
}
