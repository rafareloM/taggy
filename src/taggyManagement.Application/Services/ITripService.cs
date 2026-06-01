using taggyManagement.Application.DTOs.Trips;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface ITripService
{
    Task<Result<TripResponseDto>> CreateAsync(Guid userId, CreateTripRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TripResponseDto>>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<TripResponseDto>> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
