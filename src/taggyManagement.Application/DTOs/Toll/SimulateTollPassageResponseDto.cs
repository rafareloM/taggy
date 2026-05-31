namespace taggyManagement.Application.DTOs.Toll;

public sealed class SimulateTollPassageResponseDto
{
    public decimal PreviousBalance { get; set; }
    public decimal TollAmount { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool AutoRefillTriggered { get; set; }
}
