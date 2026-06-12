namespace taggyManagement.Application.DTOs.Fleet;

public sealed class FleetDashboardResponseDto
{
    public int TotalVehicles { get; set; }
    public int TotalTrips { get; set; }
    public decimal TotalDistanceKm { get; set; }
    public decimal TotalTollCost { get; set; }
    public decimal TotalFuelCost { get; set; }
    public decimal TotalCO2EmissionKg { get; set; }
    public decimal TotalTagSpent { get; set; }
}
