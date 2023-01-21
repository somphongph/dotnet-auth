using System.Net;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Domain.Models;
using Domain.Services.ShortLinks.Queries.GetLongUrl;
using Domain.Services.ShortLinks.Commands.CreateShortUrl;

namespace API.Controllers;

[ApiController]
[Route("v1/short-links")]
public class ShortLinksController : ControllerBase
{
    private readonly ILogger<ShortLinksController> _logger;
    private readonly IMediator _mediator;

    public ShortLinksController(ILogger<ShortLinksController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseItem<GetLongUrlResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Get([FromQuery] GetLongUrlQuery query)
    {
        var res = await _mediator.Send(query);

        return Ok(res);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseCommand<CreateShortUrlResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Create(CreateShortUrlCommand command)
    {
        var res = await _mediator.Send(command);

        return Ok(res);
    }

}
