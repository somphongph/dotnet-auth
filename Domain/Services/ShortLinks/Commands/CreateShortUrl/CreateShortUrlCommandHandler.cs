using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Repositories;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Domain.Services.ShortLinks.Commands.CreateShortUrl
{
    public class CreateShortLinkCommandHandler : IRequestHandler<CreateShortUrlCommand, ResponseCommand<CreateShortUrlResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IShortLinkRepository _shortLinkRepository;
        private readonly IShortLinkMongoRepository _mongoShortLink;

        public CreateShortLinkCommandHandler(IConfiguration configuration, IShortLinkRepository shortLinkRepository, IShortLinkMongoRepository mongoShortLink)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _shortLinkRepository = shortLinkRepository ?? throw new ArgumentNullException(nameof(shortLinkRepository));
            _mongoShortLink = mongoShortLink ?? throw new ArgumentNullException(nameof(mongoShortLink));
        }

        public async Task<ResponseCommand<CreateShortUrlResponse>> Handle(CreateShortUrlCommand req, CancellationToken cancellationToken)
        {
            var shortDomain = _configuration.GetValue<string>("ShortLinkSettings:ShortDomain");
            var expire = Int32.Parse(_configuration.GetValue<string>("ShortLinkSettings:Expire"));
            var shortKey = await GenerateKeyAsync();
            var expiredOn = DateTime.UtcNow.AddSeconds(expire);

            var shortUrl = $"{shortDomain}/{shortKey}";
            var shortLink = new ShortLink()
            {
                ShortKey = shortKey,
                ShortUrl = shortUrl,
                LongUrl = req.LongUrl,
                ExpiredOn = expiredOn
            };

            var shortLinkMongo = new ShortLinkMongo()
            {
                ShortKey = shortKey,
                ShortUrl = shortUrl,
                LongUrl = req.LongUrl,
                ExpiredOn = expiredOn
            };

            // MSSQL Add
            await _shortLinkRepository.AddAsync(shortLink);

            // MongoDB Add
            await _mongoShortLink.AddAsync(shortLinkMongo);

            var resp = new CreateShortUrlResponse()
            {
                ShortUrl = shortUrl
            };

            return new ResponseCommand<CreateShortUrlResponse>()
            {
                IsSuccess = true,
                Data = resp
            };
        }

        private async Task<string> GenerateKeyAsync()
        {
            var shortLength = Int32.Parse(_configuration.GetValue<string>("ShortLinkSettings:ShortLength"));
            var key = RandomHelper.RandomToLower(shortLength);

            var shortLink = await _mongoShortLink.GetByShortKeyAsync(key);
            if (shortLink is not null)
            {
                key = await GenerateKeyAsync();
            }

            return key;
        }
    }
}