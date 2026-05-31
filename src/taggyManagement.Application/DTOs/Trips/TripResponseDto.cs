namespace taggyManagement.Application.DTOs.Trips;

public sealed class TripResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }
    public decimal DistanceKm { get; set; }
    public decimal TollCost { get; set; }
    public decimal FuelCost { get; set; }
    public decimal EnergyCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal CO2EmissionKg { get; set; }
    public DateTime CreatedAt { get; set; }
}
