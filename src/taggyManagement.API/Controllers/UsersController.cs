using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.Auth;
using taggyManagement.Application.DTOs.Users;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;

namespace taggyManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _userService.GetMeAsync(userIdResult.Value, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Error });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _userService.UpdateMeAsync(userIdResult.Value, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { message = result.Error });
    }

    [HttpPatch("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _userService.ChangePasswordAsync(userIdResult.Value, request, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.Error });
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _userService.DeleteMeAsync(userIdResult.Value, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(new { message = result.Error });
    }

    private Result<Guid> GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(value, out var userId))
        {
            return Result<Guid>.Ok(userId);
        }

        return Result<Guid>.Fail("Invalid user token");
    }
}