using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.TagAccounts;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;

namespace taggyManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/tag-account")]
[Produces("application/json")]
public sealed class TagAccountController : ControllerBase
{
    private readonly ITagAccountService _tagAccountService;

    public TagAccountController(ITagAccountService tagAccountService)
    {
        _tagAccountService = tagAccountService;
    }

    [HttpPost("recharge")]
    [ProducesResponseType(typeof(TagBalanceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Recharge([FromBody] RechargeRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tagAccountService.RechargeAsync(userIdResult.Value, request, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("balance")]
    [ProducesResponseType(typeof(TagBalanceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tagAccountService.GetBalanceAsync(userIdResult.Value, cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("statement")]
    [ProducesResponseType(typeof(IReadOnlyList<TransactionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatement(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tagAccountService.GetStatementAsync(userIdResult.Value, cancellationToken);
        return ToActionResult(result);
    }

    private IActionResult ToActionResult<T>(Result<T> result)
    {
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
