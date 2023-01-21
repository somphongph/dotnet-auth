
using System.ComponentModel.DataAnnotations;
using Domain.Models;
using MediatR;

namespace Domain.Services.Accounts.Commands.CreateAccount;

public class CreateAccount : IRequest<ResponseCommand<CreateAccountResponse>>
{
    [Required]
    public string Name { get; set; } = String.Empty;

    [Required]
    public string Username { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;
}
