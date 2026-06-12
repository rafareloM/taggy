using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Domain.ValueObjects;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class FleetAnalyticsRepository : IFleetAnalyticsRepository
{
    private readonly TaggyDbContext _context;

    public FleetAnalyticsRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public Task<int> CountVehiclesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.Vehicles.CountAsync(vehicle => vehicle.UserId == userId, cancellationToken);
    }

    public async Task<IReadOnlyList<Trip>> GetTripsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Where(trip => trip.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Trip>> GetTripsByPeriodAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Where(trip => trip.UserId == userId && trip.CreatedAt >= startDate && trip.CreatedAt < endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> SumTagSpentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tagAccountId = await _context.TagAccounts
            .Where(tagAccount => tagAccount.UserId == userId)
            .Select(tagAccount => (Guid?)tagAccount.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (tagAccountId is null)
        {
            return 0;
        }

        return await _context.Transactions
            .Where(transaction => transaction.TagAccountId == tagAccountId && transaction.Type == TransactionType.TollDebit)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;
    }

    public async Task<int> CountTollPassagesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trips
            .Where(trip => trip.UserId == userId)
            .SumAsync(trip => (int?)trip.TollPassageCount, cancellationToken) ?? 0;
    }
}
