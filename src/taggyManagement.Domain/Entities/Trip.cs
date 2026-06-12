namespace taggyManagement.Domain.Entities;

public class Trip
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid VehicleId { get; private set; }
    public decimal DistanceKm { get; private set; }
    public int TollPassageCount { get; private set; }
    public decimal TollCost { get; private set; }
    public decimal FuelCost { get; private set; }
    public decimal EnergyCost { get; private set; }
    public decimal TotalCost { get; private set; }
    public decimal CO2EmissionKg { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Trip()
    {
    }

    public static Trip Create(
        Guid userId,
        Guid vehicleId,
        decimal distanceKm,
        int tollPassageCount,
        decimal tollCost,
        decimal fuelCost,
        decimal energyCost,
        decimal totalCost,
        decimal co2EmissionKg)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User id is required", nameof(userId));
        if (vehicleId == Guid.Empty) throw new ArgumentException("Vehicle id is required", nameof(vehicleId));
        if (distanceKm <= 0) throw new ArgumentOutOfRangeException(nameof(distanceKm), "Distance must be greater than zero");
        if (tollPassageCount < 0) throw new ArgumentOutOfRangeException(nameof(tollPassageCount), "Toll passage count cannot be negative");
        if (tollCost < 0) throw new ArgumentOutOfRangeException(nameof(tollCost), "Toll cost cannot be negative");
        if (fuelCost < 0) throw new ArgumentOutOfRangeException(nameof(fuelCost), "Fuel cost cannot be negative");
        if (energyCost < 0) throw new ArgumentOutOfRangeException(nameof(energyCost), "Energy cost cannot be negative");
        if (totalCost < 0) throw new ArgumentOutOfRangeException(nameof(totalCost), "Total cost cannot be negative");
        if (co2EmissionKg < 0) throw new ArgumentOutOfRangeException(nameof(co2EmissionKg), "CO2 emission cannot be negative");

        return new Trip
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VehicleId = vehicleId,
            DistanceKm = distanceKm,
            TollPassageCount = tollPassageCount,
            TollCost = tollCost,
            FuelCost = fuelCost,
            EnergyCost = energyCost,
            TotalCost = totalCost,
            CO2EmissionKg = co2EmissionKg,
            CreatedAt = DateTime.UtcNow
        };
    }
}
