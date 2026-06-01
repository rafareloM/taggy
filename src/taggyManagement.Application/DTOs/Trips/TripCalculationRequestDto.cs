using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.Trips;

public sealed class TripCalculationRequestDto
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
