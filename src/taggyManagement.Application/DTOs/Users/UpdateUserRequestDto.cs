using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.Users;

public sealed class UpdateUserRequestDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
}