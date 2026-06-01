using taggyManagement.Application.DTOs.Vehicles;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Infrastructure.Services;

public sealed class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Result<IReadOnlyList<VehicleResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
        return Result<IReadOnlyList<VehicleResponseDto>>.Ok(vehicles.Select(ToDto).ToList());
    }

    public async Task<Result<VehicleResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle is null)
        {
            return Result<VehicleResponseDto>.Fail("Vehicle not found");
        }

        return Result<VehicleResponseDto>.Ok(ToDto(vehicle));
    }

    public async Task<Result<VehicleResponseDto>> CreateAsync(CreateVehicleRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingVehicle = await _vehicleRepository.GetByPlateAsync(request.Plate, cancellationToken);
        if (existingVehicle is not null)
        {
            return Result<VehicleResponseDto>.Fail("A vehicle with this plate already exists");
        }

        var validationResult = ValidateMetrics(request.Propulsion, request.FuelConsumptionKmPerLiter, request.CO2GramsPerKm, request.BatteryKwhPerKm);
        if (!validationResult.IsSuccess)
        {
            return Result<VehicleResponseDto>.Fail(validationResult.Error!);
        }

        var vehicle = new Vehicle(NormalizePlate(request.Plate), request.Brand, request.Model, request.Year, request.Propulsion);
        ApplyMetrics(vehicle, request.FuelConsumptionKmPerLiter, request.CO2GramsPerKm, request.BatteryKwhPerKm);

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        return Result<VehicleResponseDto>.Ok(ToDto(vehicle));
    }

    public async Task<Result<VehicleResponseDto>> UpdateAsync(Guid id, UpdateVehicleRequestDto request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle is null)
        {
            return Result<VehicleResponseDto>.Fail("Vehicle not found");
        }

        var existingVehicle = await _vehicleRepository.GetByPlateAsync(request.Plate, cancellationToken);
        if (existingVehicle is not null && existingVehicle.Id != vehicle.Id)
        {
            return Result<VehicleResponseDto>.Fail("A vehicle with this plate already exists");
        }

        var validationResult = ValidateMetrics(request.Propulsion, request.FuelConsumptionKmPerLiter, request.CO2GramsPerKm, request.BatteryKwhPerKm);
        if (!validationResult.IsSuccess)
        {
            return Result<VehicleResponseDto>.Fail(validationResult.Error!);
        }

        vehicle.UpdateDetails(NormalizePlate(request.Plate), request.Brand, request.Model, request.Year, request.Propulsion);
        ApplyMetrics(vehicle, request.FuelConsumptionKmPerLiter, request.CO2GramsPerKm, request.BatteryKwhPerKm);

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return Result<VehicleResponseDto>.Ok(ToDto(vehicle));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
        if (vehicle is null)
        {
            return Result.Fail("Vehicle not found");
        }

        await _vehicleRepository.DeleteAsync(vehicle, cancellationToken);

        return Result.Ok();
    }

    private static Result ValidateMetrics(PropulsionType propulsion, decimal? fuelConsumptionKmPerLiter, decimal? co2GramsPerKm, decimal? batteryKwhPerKm)
    {
        return propulsion switch
        {
            PropulsionType.Combustion when fuelConsumptionKmPerLiter is null => Result.Fail("FuelConsumptionKmPerLiter is required for combustion vehicles"),
            PropulsionType.Combustion when co2GramsPerKm is null => Result.Fail("CO2GramsPerKm is required for combustion vehicles"),
            PropulsionType.Combustion when fuelConsumptionKmPerLiter <= 0 => Result.Fail("FuelConsumptionKmPerLiter must be greater than zero"),
            PropulsionType.Combustion when co2GramsPerKm < 0 => Result.Fail("CO2GramsPerKm cannot be negative"),
            PropulsionType.Electric when batteryKwhPerKm is null => Result.Fail("BatteryKwhPerKm is required for electric vehicles"),
            PropulsionType.Electric when batteryKwhPerKm <= 0 => Result.Fail("BatteryKwhPerKm must be greater than zero"),
            PropulsionType.Hybrid => Result.Fail("Hybrid vehicles are not supported by the current vehicle metrics"),
            _ => Result.Ok()
        };
    }

    private static void ApplyMetrics(Vehicle vehicle, decimal? fuelConsumptionKmPerLiter, decimal? co2GramsPerKm, decimal? batteryKwhPerKm)
    {
        if (vehicle.Propulsion == PropulsionType.Combustion)
        {
            vehicle.SetCombustionMetrics(fuelConsumptionKmPerLiter!.Value, co2GramsPerKm!.Value);
            return;
        }

        if (vehicle.Propulsion == PropulsionType.Electric)
        {
            vehicle.SetElectricMetrics(batteryKwhPerKm!.Value);
        }
    }

    private static VehicleResponseDto ToDto(Vehicle vehicle)
    {
        return new VehicleResponseDto
        {
            Id = vehicle.Id,
            Plate = vehicle.Plate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Propulsion = vehicle.Propulsion,
            FuelConsumptionKmPerLiter = vehicle.FuelConsumptionKmPerLiter,
            CO2GramsPerKm = vehicle.CO2GramsPerKm,
            BatteryKwhPerKm = vehicle.BatteryKwhPerKm
        };
    }

    private static string NormalizePlate(string plate)
    {
        return plate.Trim().ToUpperInvariant();
    }

}
