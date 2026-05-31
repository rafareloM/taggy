using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.Toll;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;

namespace taggyManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/toll")]
[Produces("application/json")]
public sealed class TollController : ControllerBase
{
    private readonly IAutoRefillService _autoRefillService;

    public TollController(IAutoRefillService autoRefillService)
    {
        _autoRefillService = autoRefillService;
    }

    [HttpPost("simulate")]
    [ProducesResponseType(typeof(SimulateTollPassageResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Simulate([FromBody] SimulateTollPassageRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _autoRefillService.SimulateTollPassageAsync(userIdResult.Value, request, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error == "Tag account not found" ? NotFound(new { message = result.Error }) : BadRequest(new { message = result.Error });
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
