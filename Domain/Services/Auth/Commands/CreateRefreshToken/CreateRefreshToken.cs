using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Domain.Services.Auth.Shared;
using MediatR;

namespace Domain.Services.Auth.Commands.CreateRefreshToken;

public class CreateRefreshToken : IRequest<ResponseCommand<TokenJwt>>
{
    [Required]
    public string RefreshToken { get; set; } = String.Empty;
}
