using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.TagAccounts;

public sealed class RechargeRequestDto
{
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}
