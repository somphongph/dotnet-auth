using Domain.Extensions;
using Domain.Interfaces.Repositories;
using Domain.Models;
using MediatR;

namespace Domain.Services.Accounts.Queries.GetAccountById;

public class GetAccountByIdHandler : IRequestHandler<GetAccountById, ResponseItem<GetAccountByIdResponse>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<ResponseItem<GetAccountByIdResponse>> Handle(GetAccountById req, CancellationToken cancellationToken)
    {
        // Get Account
        var acc = await _accountRepository.GetByIdAsync(req.UserId);
        if (acc == null)
        {
            return new ResponseItem<GetAccountByIdResponse>()
            {
                IsSuccess = false,
                Code = ResponseStatus.NotFound.Code(),
                Message = ResponseStatus.NotFound.NameString(),
                Data = null
            };
        }

        // Response
        var res = new GetAccountByIdResponse
        {
            Name = acc.Name
        };

        return new ResponseItem<GetAccountByIdResponse>()
        {
            IsSuccess = true,
            Code = ResponseStatus.Success.Code(),
            Message = ResponseStatus.Success.NameString(),
            Data = res
        };
    }
}
