using System.ComponentModel.DataAnnotations;

namespace taggyManagement.Application.DTOs.Auth;

public sealed class RegisterUserRequestDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}