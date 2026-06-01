using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface ITripRepository
{
    Task<IReadOnlyList<Trip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Trip trip, CancellationToken cancellationToken = default);
}
