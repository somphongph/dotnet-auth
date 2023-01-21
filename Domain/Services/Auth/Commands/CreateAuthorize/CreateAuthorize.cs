using System.ComponentModel.DataAnnotations;
using Domain.Models;
using MediatR;

namespace Domain.Services.Auth.Commands.CreateAuthorize;

public class CreateAuthorize : IRequest<ResponseCommand<CreateAuthorizeResponse>>
{
    [Required]
    public string Username { get; set; } = String.Empty;

    [Required]
    public string Password { get; set; } = String.Empty;

    [Required]
    public string CodeChallenge { get; set; } = String.Empty;
}
