
using Domain.Entities;
using Domain.Extensions;
using Domain.Helpers;
using Domain.Interfaces.Repositories;
using Domain.Models;
using MediatR;

namespace Domain.Services.Accounts.Commands.CreateAccount;

public class CreateAccountHandler : IRequestHandler<CreateAccount, ResponseCommand<CreateAccountResponse>>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<ResponseCommand<CreateAccountResponse>> Handle(CreateAccount req, CancellationToken cancellationToken)
    {
        // Validate username
        var accValidate = await _accountRepository.GetByUsernameAsync(req.Username);
        if (accValidate != null)
        {
            return new ResponseCommand<CreateAccountResponse>()
            {
                IsSuccess = false,
                Code = ResponseStatus.Duplicated.Code(),
                Message = ResponseStatus.Duplicated.NameString(),
                Data = null
            };
        }

        // Add
        var salt = HashHelper.GenerateSalt();
        var pass = HashHelper.GenerateHash(req.Password, salt);
        var acc = new Account
        {
            Name = req.Name,
            Username = req.Username,
            Password = pass,
            PasswordSalt = salt,
            UserRole = AccountRole.User.Code(),
            LastSigninOn = DateTime.UtcNow
        };
        await _accountRepository.AddAsync(acc);

        // Response
        var res = new CreateAccountResponse
        {
            UserId = acc.Id.ToString()
        };

        return new ResponseCommand<CreateAccountResponse>()
        {
            IsSuccess = true,
            Code = ResponseStatus.Success.Code(),
            Message = ResponseStatus.Success.NameString(),
            Data = res
        };
    }
}
