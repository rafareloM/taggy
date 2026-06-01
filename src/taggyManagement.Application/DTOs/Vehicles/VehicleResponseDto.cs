using System;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Application.DTOs.Vehicles;

public sealed class VehicleResponseDto
{
    public Guid Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public PropulsionType Propulsion { get; set; }
    public decimal? FuelConsumptionKmPerLiter { get; set; }
    public decimal? CO2GramsPerKm { get; set; }
    public decimal? BatteryKwhPerKm { get; set; }
}
