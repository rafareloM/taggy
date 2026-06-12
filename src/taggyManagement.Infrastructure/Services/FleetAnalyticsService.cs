using taggyManagement.Application.DTOs.Fleet;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Interfaces;

namespace taggyManagement.Infrastructure.Services;

public sealed class FleetAnalyticsService : IFleetAnalyticsService
{
    private const decimal Co2AvoidedPerTollPassageKg = 0.15m;
    private const int MinutesSavedPerTollPassage = 5;

    private readonly IFleetAnalyticsRepository _fleetAnalyticsRepository;

    public FleetAnalyticsService(IFleetAnalyticsRepository fleetAnalyticsRepository)
    {
        _fleetAnalyticsRepository = fleetAnalyticsRepository;
    }

    public async Task<Result<FleetDashboardResponseDto>> GetDashboardAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userValidation = ValidateUser(userId);
        if (!userValidation.IsSuccess)
        {
            return Result<FleetDashboardResponseDto>.Fail(userValidation.Error!);
        }

        var trips = await _fleetAnalyticsRepository.GetTripsAsync(userId, cancellationToken);
        var totalVehicles = await _fleetAnalyticsRepository.CountVehiclesAsync(userId, cancellationToken);
        var totalTagSpent = await _fleetAnalyticsRepository.SumTagSpentAsync(userId, cancellationToken);

        return Result<FleetDashboardResponseDto>.Ok(new FleetDashboardResponseDto
        {
            TotalVehicles = totalVehicles,
            TotalTrips = trips.Count,
            TotalDistanceKm = trips.Sum(trip => trip.DistanceKm),
            TotalTollCost = trips.Sum(trip => trip.TollCost),
            TotalFuelCost = trips.Sum(trip => trip.FuelCost),
            TotalCO2EmissionKg = trips.Sum(trip => trip.CO2EmissionKg),
            TotalTagSpent = totalTagSpent
        });
    }

    public async Task<Result<MonthlyFleetAnalyticsResponseDto>> GetMonthlyAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var userValidation = ValidateUser(userId);
        if (!userValidation.IsSuccess)
        {
            return Result<MonthlyFleetAnalyticsResponseDto>.Fail(userValidation.Error!);
        }

        if (year < 1 || year > 9999)
        {
            return Result<MonthlyFleetAnalyticsResponseDto>.Fail("Year must be between 1 and 9999");
        }

        if (month < 1 || month > 12)
        {
            return Result<MonthlyFleetAnalyticsResponseDto>.Fail("Month must be between 1 and 12");
        }

        var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1);
        var trips = await _fleetAnalyticsRepository.GetTripsByPeriodAsync(userId, startDate, endDate, cancellationToken);

        return Result<MonthlyFleetAnalyticsResponseDto>.Ok(new MonthlyFleetAnalyticsResponseDto
        {
            Month = month,
            Year = year,
            VehiclesUsed = trips.Select(trip => trip.VehicleId).Distinct().Count(),
            TripCount = trips.Count,
            TotalDistanceKm = trips.Sum(trip => trip.DistanceKm),
            TotalTollCost = trips.Sum(trip => trip.TollCost),
            TotalFuelCost = trips.Sum(trip => trip.FuelCost),
            TotalCO2EmissionKg = trips.Sum(trip => trip.CO2EmissionKg)
        });
    }

    public async Task<Result<FleetEnvironmentResponseDto>> GetEnvironmentAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userValidation = ValidateUser(userId);
        if (!userValidation.IsSuccess)
        {
            return Result<FleetEnvironmentResponseDto>.Fail(userValidation.Error!);
        }

        var totalTollPassages = await _fleetAnalyticsRepository.CountTollPassagesAsync(userId, cancellationToken);
        return Result<FleetEnvironmentResponseDto>.Ok(new FleetEnvironmentResponseDto
        {
            TotalTollPassages = totalTollPassages,
            CO2AvoidedKg = totalTollPassages * Co2AvoidedPerTollPassageKg
        });
    }

    public async Task<Result<FleetTimeSavingsResponseDto>> GetTimeSavingsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userValidation = ValidateUser(userId);
        if (!userValidation.IsSuccess)
        {
            return Result<FleetTimeSavingsResponseDto>.Fail(userValidation.Error!);
        }

        var totalTollPassages = await _fleetAnalyticsRepository.CountTollPassagesAsync(userId, cancellationToken);
        var timeSavedMinutes = totalTollPassages * MinutesSavedPerTollPassage;

        return Result<FleetTimeSavingsResponseDto>.Ok(new FleetTimeSavingsResponseDto
        {
            TotalTollPassages = totalTollPassages,
            TimeSavedMinutes = timeSavedMinutes,
            TimeSavedHours = Math.Round(timeSavedMinutes / 60m, 2),
            TimeSavedDays = Math.Round(timeSavedMinutes / 1440m, 2)
        });
    }

    private static Result ValidateUser(Guid userId)
    {
        return userId == Guid.Empty ? Result.Fail("UserId is required") : Result.Ok();
    }
}
