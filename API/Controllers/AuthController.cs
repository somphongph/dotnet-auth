using System.Net;
using Domain.Models;
using Domain.Services.Auth.Commands.CreateAuthorize;
using Domain.Services.Auth.Commands.CreateRefreshToken;
using Domain.Services.Auth.Commands.CreateToken;
using Domain.Services.Auth.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost("authorize")]
    [ProducesResponseType(typeof(ResponseCommand<CreateAuthorizeResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Authorize(CreateAuthorize command)
    {
        var res = await _mediator.Send(command);

        return Ok(res);
    }

    [HttpPost("token")]
    [ProducesResponseType(typeof(ResponseCommand<TokenJwt>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Token(CreateToken command)
    {
        var res = await _mediator.Send(command);

        return Ok(res);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseCommand<TokenJwt>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> RefreshToken(CreateRefreshToken command)
    {
        var res = await _mediator.Send(command);

        return Ok(res);
    }
}

