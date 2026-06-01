using taggyManagement.Application.DTOs.AutoRefill;
using taggyManagement.Application.DTOs.Toll;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface IAutoRefillService
{
    Task<Result<AutoRefillSettingsResponseDto>> GetSettingsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<AutoRefillSettingsResponseDto>> ConfigureAsync(Guid userId, ConfigureAutoRefillRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<SimulateTollPassageResponseDto>> SimulateTollPassageAsync(Guid userId, SimulateTollPassageRequestDto request, CancellationToken cancellationToken = default);
}
