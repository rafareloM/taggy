using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Application.DTOs.Vehicles;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface IVehicleService
{
    Task<Result<IReadOnlyList<VehicleResponseDto>>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> CreateAsync(Guid userId, CreateVehicleRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<BulkCreateVehicleResponseDto>> BulkCreateAsync(Guid userId, IReadOnlyList<BulkCreateVehicleRequestDto> request, CancellationToken cancellationToken = default);
    Task<Result<VehicleResponseDto>> UpdateAsync(Guid userId, Guid id, UpdateVehicleRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
