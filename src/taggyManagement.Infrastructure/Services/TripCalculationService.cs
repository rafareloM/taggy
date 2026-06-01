using taggyManagement.Application.DTOs.Trips;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Infrastructure.Services;

public sealed class TripCalculationService : ITripCalculationService
{
    private readonly IVehicleRepository _vehicleRepository;

    public TripCalculationService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Result<TripCalculationResponseDto>> CalculateAsync(TripCalculationRequestDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = ValidateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return Result<TripCalculationResponseDto>.Fail(validationResult.Error!);
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return Result<TripCalculationResponseDto>.Fail("Vehicle not found");
        }

        var tollCost = request.TollPrices.Sum();
        var fuelCost = 0m;
        var energyCost = 0m;
        var co2EmissionKg = 0m;

        var metricsResult = CalculateVehicleCosts(vehicle, request, ref fuelCost, ref energyCost, ref co2EmissionKg);
        if (!metricsResult.IsSuccess)
        {
            return Result<TripCalculationResponseDto>.Fail(metricsResult.Error!);
        }

        return Result<TripCalculationResponseDto>.Ok(new TripCalculationResponseDto
        {
            VehicleId = request.VehicleId,
            DistanceKm = request.DistanceKm,
            TollCost = tollCost,
            FuelCost = fuelCost,
            EnergyCost = energyCost,
            TotalCost = tollCost + fuelCost + energyCost,
            CO2EmissionKg = co2EmissionKg
        });
    }

    private static Result ValidateRequest(TripCalculationRequestDto request)
    {
        if (request.VehicleId == Guid.Empty)
        {
            return Result.Fail("VehicleId is required");
        }

        if (request.DistanceKm <= 0)
        {
            return Result.Fail("DistanceKm must be greater than zero");
        }

        if (request.FuelPrice < 0)
        {
            return Result.Fail("FuelPrice cannot be negative");
        }

        if (request.EnergyPrice < 0)
        {
            return Result.Fail("EnergyPrice cannot be negative");
        }

        if (request.TollPrices is null)
        {
            return Result.Fail("TollPrices is required");
        }

        if (request.TollPrices.Any(tollPrice => tollPrice < 0))
        {
            return Result.Fail("TollPrices cannot contain negative values");
        }

        return Result.Ok();
    }

    private static Result CalculateVehicleCosts(Vehicle vehicle, TripCalculationRequestDto request, ref decimal fuelCost, ref decimal energyCost, ref decimal co2EmissionKg)
    {
        if (vehicle.Propulsion == PropulsionType.Combustion)
        {
            if (vehicle.FuelConsumptionKmPerLiter is null or <= 0)
            {
                return Result.Fail("FuelConsumptionKmPerLiter is required for combustion vehicles");
            }

            if (vehicle.CO2GramsPerKm is null or < 0)
            {
                return Result.Fail("CO2GramsPerKm is required for combustion vehicles");
            }

            var fuelConsumedLiters = request.DistanceKm / vehicle.FuelConsumptionKmPerLiter.Value;
            fuelCost = fuelConsumedLiters * request.FuelPrice;
            co2EmissionKg = request.DistanceKm * vehicle.CO2GramsPerKm.Value / 1000;
            return Result.Ok();
        }

        if (vehicle.Propulsion == PropulsionType.Electric)
        {
            if (vehicle.BatteryKwhPerKm is null or <= 0)
            {
                return Result.Fail("BatteryKwhPerKm is required for electric vehicles");
            }

            var energyConsumedKwh = request.DistanceKm * vehicle.BatteryKwhPerKm.Value;
            energyCost = energyConsumedKwh * request.EnergyPrice;
            return Result.Ok();
        }

        return Result.Fail("Hybrid vehicles are not supported by the current trip calculation");
    }
}
