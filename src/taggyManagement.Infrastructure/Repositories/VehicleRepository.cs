using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly TaggyDbContext _context;

    public VehicleRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .OrderBy(vehicle => vehicle.Brand)
            .ThenBy(vehicle => vehicle.Model)
            .ThenBy(vehicle => vehicle.Plate)
            .ToListAsync(cancellationToken);
    }

    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == id, cancellationToken);
    }

    public Task<Vehicle?> GetByPlateAsync(string plate, CancellationToken cancellationToken = default)
    {
        var normalizedPlate = NormalizePlate(plate);
        return _context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Plate.ToUpper() == normalizedPlate, cancellationToken);
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static string NormalizePlate(string plate)
    {
        return plate.Trim().ToUpperInvariant();
    }
}
