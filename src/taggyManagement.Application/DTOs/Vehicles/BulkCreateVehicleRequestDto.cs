using System.ComponentModel.DataAnnotations;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Application.DTOs.Vehicles;

public sealed class BulkCreateVehicleRequestDto
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Plate { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 1)]
    public string Model { get; set; } = string.Empty;

    [Range(1886, 2100)]
    public int Year { get; set; }

    [Required]
    public PropulsionType Propulsion { get; set; }

    public decimal? FuelConsumptionKmPerLiter { get; set; }
    public decimal? CO2GramsPerKm { get; set; }
    public decimal? BatteryKwhPerKm { get; set; }
}
