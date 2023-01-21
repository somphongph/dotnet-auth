using System.ComponentModel.DataAnnotations;
using Domain.Models;
using MediatR;

namespace Domain.Services.ShortLinks.Commands.CreateShortUrl
{
    public class CreateShortUrlCommand : IRequest<ResponseCommand<CreateShortUrlResponse>>
    {
        [Required]
        public string LongUrl { get; set; } = String.Empty;
    }
}