using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vehicle>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByPlateAsync(string plate, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByPlateAsync(Guid userId, string plate, CancellationToken cancellationToken = default);
    Task<IReadOnlySet<string>> GetExistingPlatesAsync(Guid userId, IEnumerable<string> plates, CancellationToken cancellationToken = default);
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
}
