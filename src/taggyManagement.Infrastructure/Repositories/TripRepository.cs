using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class TripRepository : ITripRepository
{
    private readonly TaggyDbContext _context;

    public TripRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Trip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Where(trip => trip.UserId == userId)
            .OrderByDescending(trip => trip.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Trip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Trips.FirstOrDefaultAsync(trip => trip.Id == id, cancellationToken);
    }

    public async Task AddAsync(Trip trip, CancellationToken cancellationToken = default)
    {
        await _context.Trips.AddAsync(trip, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
