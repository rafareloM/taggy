namespace taggyManagement.Application.DTOs.Trips;

using System.ComponentModel.DataAnnotations;

public sealed class CreateTripRequestDto
{
    [Required]
    public Guid VehicleId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal DistanceKm { get; set; }

    [Range(0, double.MaxValue)]
    public decimal FuelPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal EnergyPrice { get; set; }

    [Required]
    public List<decimal> TollPrices { get; set; } = new();
}
