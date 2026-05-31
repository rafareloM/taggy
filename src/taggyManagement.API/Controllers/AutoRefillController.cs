using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.AutoRefill;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;

namespace taggyManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/auto-refill")]
[Produces("application/json")]
public sealed class AutoRefillController : ControllerBase
{
    private readonly IAutoRefillService _autoRefillService;

    public AutoRefillController(IAutoRefillService autoRefillService)
    {
        _autoRefillService = autoRefillService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AutoRefillSettingsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _autoRefillService.GetSettingsAsync(userIdResult.Value, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Error });
    }

    [HttpPost]
    [ProducesResponseType(typeof(AutoRefillSettingsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Configure([FromBody] ConfigureAutoRefillRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _autoRefillService.ConfigureAsync(userIdResult.Value, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { message = result.Error });
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
