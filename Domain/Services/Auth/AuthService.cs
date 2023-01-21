using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Helpers;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Services.Auth.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAccountRepository _accountRepository;

    public AuthService(
        IConfiguration configuration,
        IAccountRepository accountRepository
    )
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<TokenJwt?> GenerateTokenAsync(string userId)
    {
        var acc = await _accountRepository.GetByIdAsync(userId);
        if (acc == null)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, acc.Id.ToString()),
            new Claim(ClaimTypes.Role, acc.UserRole)
        };

        var accessTokenExpire = _configuration.GetValue<string>("JWT:AccessTokenExpire");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:SecurityKey")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(accessTokenExpire));

        // RefreshToken
        var refreshToken = $"{RandomHelper.RandomKey(32)}{Guid.NewGuid().ToString("N")}";

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds,
            issuer: _configuration.GetValue<string>("JWT:Issuer"),
            audience: _configuration.GetValue<string>("JWT:Audience")
        );

        return new TokenJwt
        {
            TokenType = "Bearer",
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            AccessTokenExpire = accessTokenExpire,
            RefreshToken = refreshToken,
        };
    }
}
