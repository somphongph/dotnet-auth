using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Domain.Services.Auth.Shared;
using MediatR;

namespace Domain.Services.Auth.Commands.CreateToken;

public class CreateToken : IRequest<ResponseCommand<TokenJwt>>
{
    [Required]
    public string AuthorizeCode { get; set; } = String.Empty;

    [Required]
    public string CodeVerifier { get; set; } = String.Empty;
}
