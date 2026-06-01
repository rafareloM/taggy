using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class AutoRefillSettingsRepository : IAutoRefillSettingsRepository
{
    private readonly TaggyDbContext _context;

    public AutoRefillSettingsRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public Task<AutoRefillSettings?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.AutoRefillSettings.FirstOrDefaultAsync(settings => settings.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(AutoRefillSettings settings, CancellationToken cancellationToken = default)
    {
        await _context.AutoRefillSettings.AddAsync(settings, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AutoRefillSettings settings, CancellationToken cancellationToken = default)
    {
        _context.AutoRefillSettings.Update(settings);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
