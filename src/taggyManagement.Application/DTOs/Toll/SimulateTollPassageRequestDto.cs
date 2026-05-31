using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.Toll;

public sealed class SimulateTollPassageRequestDto
{
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;
}
