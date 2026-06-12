using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface IFleetAnalyticsRepository
{
    Task<int> CountVehiclesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Trip>> GetTripsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Trip>> GetTripsByPeriodAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<decimal> SumTagSpentAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountTollPassagesAsync(Guid userId, CancellationToken cancellationToken = default);
}
