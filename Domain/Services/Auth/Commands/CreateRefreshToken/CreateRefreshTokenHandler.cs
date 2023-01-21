using Domain.Extensions;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Services.Auth.Shared;
using MediatR;

namespace Domain.Services.Auth.Commands.CreateRefreshToken;

public class CreateRefreshTokenHandler : IRequestHandler<CreateRefreshToken, ResponseCommand<TokenJwt>>
{
    private readonly IAuthService _authService;
    private readonly IAuthenMongoRepository _authenMongoRepository;

    public CreateRefreshTokenHandler(
        IAuthService authService,
        IAuthenMongoRepository authenRepository)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _authenMongoRepository = authenRepository ?? throw new ArgumentNullException(nameof(authenRepository));
    }

    public async Task<ResponseCommand<TokenJwt>> Handle(CreateRefreshToken req, CancellationToken cancellationToken)
    {
        var refresh = await _authenMongoRepository.GetByRefreshTokenAsync(req.RefreshToken);
        if (refresh == null)
        {
            return new ResponseCommand<TokenJwt>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // Generate New Token
        var res = await _authService.GenerateTokenAsync(refresh.UserId);
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

        // Update new RefreshToken
        refresh.RefreshToken = res.RefreshToken;
        await _authenMongoRepository.UpdateAsync(refresh);

        return new ResponseCommand<TokenJwt>()
        {
            IsSuccess = true,
            Code = ResponseStatus.Success.Code(),
            Message = ResponseStatus.Success.NameString(),
            Data = res
        };
    }
}
