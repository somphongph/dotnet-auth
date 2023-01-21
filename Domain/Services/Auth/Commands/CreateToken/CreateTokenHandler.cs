using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.CacheRepositories;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Services.Auth.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Domain.Services.Auth.Commands.CreateToken;

public class CreateTokenHandler : IRequestHandler<CreateToken, ResponseCommand<TokenJwt>>
{
    private readonly IConfiguration _configuration;
    private readonly ICacheRepository _cacheRepository;
    private readonly IAuthService _authService;
    private readonly IAuthenMongoRepository _authenMongoRepository;

    public CreateTokenHandler(
        IConfiguration configuration,
        ICacheRepository cacheRepository,
        IAuthService authService,
        IAuthenMongoRepository authenMongoRepository)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _authenMongoRepository = authenMongoRepository ?? throw new ArgumentNullException(nameof(authenMongoRepository));
    }

    public async Task<ResponseCommand<TokenJwt>> Handle(CreateToken req, CancellationToken cancellationToken)
    {
        // Get Authen Session
        var session = await _cacheRepository.GetCacheAsync<AuthenSession>(req.AuthorizeCode);
        if (session == null)
        {
            return new ResponseCommand<TokenJwt>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // Validate verifier
        byte[] bytes = Encoding.UTF8.GetBytes(req.CodeVerifier);
        bytes = SHA256.Create().ComputeHash(bytes);
        var challenge = Convert.ToBase64String(bytes);
        if (challenge != session.CodeChallenge)
        {
            return new ResponseCommand<TokenJwt>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // GenerateToken
        var res = await _authService.GenerateTokenAsync(session.UserId);
        if (res == null)
        {
            return new ResponseCommand<TokenJwt>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = res,
            };
        }

        // Save RefreshToken
        var refreshTokenExpire = _configuration.GetValue<string>("JWT:RefreshTokenExpire");
        var authen = new AuthenMongo()
        {
            UserId = session.UserId,
            RefreshToken = res.RefreshToken,
            ExpiredOn = DateTime.UtcNow.AddSeconds(Convert.ToDouble(refreshTokenExpire)),
        };
        await _authenMongoRepository.AddAsync(authen);

        return new ResponseCommand<TokenJwt>()
        {
            IsSuccess = true,
            Code = ResponseStatus.Success.Code(),
            Message = ResponseStatus.Success.NameString(),
            Data = res,
        };
    }
}
