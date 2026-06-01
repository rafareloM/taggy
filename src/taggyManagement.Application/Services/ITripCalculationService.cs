using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Application.DTOs.Trips;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface ITripCalculationService
{
    Task<Result<TripCalculationResponseDto>> CalculateAsync(TripCalculationRequestDto request, CancellationToken cancellationToken = default);
}
