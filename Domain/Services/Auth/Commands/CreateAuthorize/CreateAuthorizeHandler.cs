using Domain.Extensions;
using Domain.Helpers;
using Domain.Interfaces.CacheRepositories;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Services.Auth.Shared;
using MediatR;

namespace Domain.Services.Auth.Commands.CreateAuthorize;

public class CreateAuthorizeHandler : IRequestHandler<CreateAuthorize, ResponseCommand<CreateAuthorizeResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICacheRepository _cacheRepository;

    public CreateAuthorizeHandler(
        IAccountRepository accountRepository,
        ICacheRepository cacheRepository
    )
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
    }

    public async Task<ResponseCommand<CreateAuthorizeResponse>> Handle(CreateAuthorize req, CancellationToken cancellationToken)
    {
        // Get User Login
        var acc = await _accountRepository.GetByUsernameAsync(req.Username);
        if (acc == null)
        {
            return new ResponseCommand<CreateAuthorizeResponse>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // Validate Password
        var pass = HashHelper.GenerateHash(req.Password, acc.PasswordSalt);
        if (pass != acc.Password)
        {
            return new ResponseCommand<CreateAuthorizeResponse>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // Create Authen Session
        var authorizeCode = Guid.NewGuid().ToString("N");
        var authen = new AuthenSession
        {
            CodeChallenge = req.CodeChallenge,
            UserId = acc.Id.ToString()
        };
        await _cacheRepository.AddCacheShortAsync(authorizeCode, authen);

        // Update LastSignin
        acc.LastSigninOn = DateTime.UtcNow;
        await _accountRepository.UpdateAsync(acc);

        var res = new CreateAuthorizeResponse()
        {
            AuthorizeCode = authorizeCode
        };

        return new ResponseCommand<CreateAuthorizeResponse>()
        {
            IsSuccess = true,
            Code = ResponseStatus.Success.Code(),
            Message = ResponseStatus.Success.NameString(),
            Data = res
        };
    }

}
