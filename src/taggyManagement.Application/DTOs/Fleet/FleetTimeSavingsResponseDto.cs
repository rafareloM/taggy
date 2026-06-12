namespace taggyManagement.Application.DTOs.Fleet;

public sealed class FleetTimeSavingsResponseDto
{
    public int TotalTollPassages { get; set; }
    public int TimeSavedMinutes { get; set; }
    public decimal TimeSavedHours { get; set; }
    public decimal TimeSavedDays { get; set; }
}
