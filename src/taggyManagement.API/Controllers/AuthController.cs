using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.Auth;
using taggyManagement.Application.Services;

namespace taggyManagement.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _userService.LoginAsync(request, cancellationToken);
        if (!result.IsSuccess)
        {
            return Unauthorized(new { message = result.Error });
        }

        return Ok(result.Value);
    }
}