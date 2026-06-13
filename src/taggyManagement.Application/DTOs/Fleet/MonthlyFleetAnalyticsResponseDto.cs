namespace taggyManagement.Application.DTOs.Fleet;

public sealed class MonthlyFleetAnalyticsResponseDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int VehiclesUsed { get; set; }
    public int TripCount { get; set; }
    public decimal TotalDistanceKm { get; set; }
    public decimal TotalTollCost { get; set; }
    public decimal TotalFuelCost { get; set; }
    public decimal TotalCO2EmissionKg { get; set; }
    public int TollPassageCount { get; set; }
    public decimal CO2AvoidedKg { get; set; }
    public int TimeSavedMinutes { get; set; }
    public decimal TimeSavedHours { get; set; }
}
