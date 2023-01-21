using System.ComponentModel.DataAnnotations;
using Domain.Models;
using MediatR;

namespace Domain.Services.Accounts.Queries.GetAccountById;

public class GetAccountById : IRequest<ResponseItem<GetAccountByIdResponse>>
{
    [Required]
    public string UserId { get; set; } = String.Empty;

}
