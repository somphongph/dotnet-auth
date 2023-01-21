using System.Net;
using Domain.Models;
using Domain.Services.Accounts.Commands.CreateAccount;
using Domain.Services.Accounts.Queries.GetAccountById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/v1/accounts")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AccountsController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [Authorize(Roles = "admin, superuser")]
    [ProducesResponseType(typeof(ResponseCommand<CreateAccount>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Create(CreateAccount command)
    {
        var res = await _mediator.Send(command);

        return Ok(res);
    }

    [HttpGet("{userId}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseCommand<GetAccountById>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Get([FromRoute] GetAccountById query)
    {
        var res = await _mediator.Send(query);

        return Ok(res);
    }
}

