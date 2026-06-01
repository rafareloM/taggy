using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taggyManagement.Application.DTOs.Trips;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;

namespace taggyManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/trips")]
public sealed class TripsController : ControllerBase
{
    private readonly ITripCalculationService _tripCalculationService;
    private readonly ITripService _tripService;

    public TripsController(ITripCalculationService tripCalculationService, ITripService tripService)
    {
        _tripCalculationService = tripCalculationService;
        _tripService = tripService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTripRequestDto request, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tripService.CreateAsync(userIdResult.Value, request, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error == "Vehicle not found" ? NotFound(new { message = result.Error }) : BadRequest(new { message = result.Error });
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TripResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tripService.GetByUserAsync(userIdResult.Value, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { message = result.Error });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TripResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Unauthorized(new { message = userIdResult.Error });
        }

        var result = await _tripService.GetByIdAsync(userIdResult.Value, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Error });
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] TripCalculationRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _tripCalculationService.CalculateAsync(request, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error == "Vehicle not found" ? NotFound(new { message = result.Error }) : BadRequest(new { message = result.Error });
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
