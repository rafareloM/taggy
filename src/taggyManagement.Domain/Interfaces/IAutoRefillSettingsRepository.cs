using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface IAutoRefillSettingsRepository
{
    Task<AutoRefillSettings?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(AutoRefillSettings settings, CancellationToken cancellationToken = default);
    Task UpdateAsync(AutoRefillSettings settings, CancellationToken cancellationToken = default);
}
