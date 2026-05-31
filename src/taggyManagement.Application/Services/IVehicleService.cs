using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Application.DTOs.Vehicles;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface IVehicleService
{
    Task<Result<IReadOnlyList<VehicleResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> CreateAsync(CreateVehicleRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> UpdateAsync(Guid id, UpdateVehicleRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
