using Domain.Services.Auth.Shared;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<TokenJwt?> GenerateTokenAsync(string userId);
}
