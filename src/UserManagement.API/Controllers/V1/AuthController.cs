using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.Features.Auth.Commands.Register;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManagement.Application.Common.Interfaces.ITenantContext _tenantContext;

    public AuthController(IMediator mediator, UserManagement.Application.Common.Interfaces.ITenantContext tenantContext)
    {
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResult>> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return NotFound();

        var query = new GetUserProfileQuery { UserId = Guid.Parse(userId) };
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        // In a real application, you might want to invalidate the token
        // For now, just return OK as the client should remove the token
        return Ok();
    }

    [HttpGet("tenant-info")]
        public IActionResult GetTenantInfo()
        {
            return Ok(new { _tenantContext.TenantId , _tenantContext.TenantCode });
        }
}