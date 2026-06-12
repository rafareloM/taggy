using taggyManagement.Application.DTOs.Fleet;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface IFleetAnalyticsService
{
    Task<Result<FleetDashboardResponseDto>> GetDashboardAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<MonthlyFleetAnalyticsResponseDto>> GetMonthlyAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default);
    Task<Result<FleetEnvironmentResponseDto>> GetEnvironmentAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<FleetTimeSavingsResponseDto>> GetTimeSavingsAsync(Guid userId, CancellationToken cancellationToken = default);
}
