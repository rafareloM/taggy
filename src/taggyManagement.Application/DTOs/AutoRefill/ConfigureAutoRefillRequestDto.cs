using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.AutoRefill;

public sealed class ConfigureAutoRefillRequestDto
{
    public bool Enabled { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal MinimumBalance { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal RechargeAmount { get; set; }
}
