using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseManagement.Identity.Application.Commands;
using ExpenseManagement.Identity.Application.DTOs;

namespace ExpenseManagement.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {Email}", request.Email);

        var command = new LoginCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for user: {Email}", request.Email);
            return Unauthorized(result);
        }

        _logger.LogInformation("Login successful for user: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        // TODO: Implement refresh token logic
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // TODO: Implement logout logic (revoke refresh token)
        return Ok();
    }
}