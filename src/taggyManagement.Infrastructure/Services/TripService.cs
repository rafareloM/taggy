using taggyManagement.Application.DTOs.Trips;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;

namespace taggyManagement.Infrastructure.Services;

public sealed class TripService : ITripService
{
    private readonly ITripCalculationService _tripCalculationService;
    private readonly ITripRepository _tripRepository;

    public TripService(ITripCalculationService tripCalculationService, ITripRepository tripRepository)
    {
        _tripCalculationService = tripCalculationService;
        _tripRepository = tripRepository;
    }

    public async Task<Result<TripResponseDto>> CreateAsync(Guid userId, CreateTripRequestDto request, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<TripResponseDto>.Fail("UserId is required");
        }

        var calculationRequest = new TripCalculationRequestDto
        {
            VehicleId = request.VehicleId,
            DistanceKm = request.DistanceKm,
            FuelPrice = request.FuelPrice,
            EnergyPrice = request.EnergyPrice,
            TollPrices = request.TollPrices
        };

        var calculationResult = await _tripCalculationService.CalculateAsync(calculationRequest, cancellationToken);
        if (!calculationResult.IsSuccess)
        {
            return Result<TripResponseDto>.Fail(calculationResult.Error!);
        }

        var calculation = calculationResult.Value!;
        var trip = Trip.Create(
            userId,
            calculation.VehicleId,
            calculation.DistanceKm,
            calculation.TollCost,
            calculation.FuelCost,
            calculation.EnergyCost,
            calculation.TotalCost,
            calculation.CO2EmissionKg);

        await _tripRepository.AddAsync(trip, cancellationToken);

        return Result<TripResponseDto>.Ok(ToDto(trip));
    }

    public async Task<Result<IReadOnlyList<TripResponseDto>>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyList<TripResponseDto>>.Fail("UserId is required");
        }

        var trips = await _tripRepository.GetByUserIdAsync(userId, cancellationToken);
        return Result<IReadOnlyList<TripResponseDto>>.Ok(trips.Select(ToDto).ToList());
    }

    public async Task<Result<TripResponseDto>> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<TripResponseDto>.Fail("UserId is required");
        }

        var trip = await _tripRepository.GetByIdAsync(id, cancellationToken);
        if (trip is null || trip.UserId != userId)
        {
            return Result<TripResponseDto>.Fail("Trip not found");
        }

        return Result<TripResponseDto>.Ok(ToDto(trip));
    }

    private static TripResponseDto ToDto(Trip trip)
    {
        return new TripResponseDto
        {
            Id = trip.Id,
            UserId = trip.UserId,
            VehicleId = trip.VehicleId,
            DistanceKm = trip.DistanceKm,
            TollCost = trip.TollCost,
            FuelCost = trip.FuelCost,
            EnergyCost = trip.EnergyCost,
            TotalCost = trip.TotalCost,
            CO2EmissionKg = trip.CO2EmissionKg,
            CreatedAt = trip.CreatedAt
        };
    }
}
