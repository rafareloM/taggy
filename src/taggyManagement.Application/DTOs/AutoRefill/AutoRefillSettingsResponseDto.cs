namespace taggyManagement.Application.DTOs.AutoRefill;

public sealed class AutoRefillSettingsResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool Enabled { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal RechargeAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
