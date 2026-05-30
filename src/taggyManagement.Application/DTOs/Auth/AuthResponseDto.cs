namespace taggyManagement.Application.DTOs.Auth;

public sealed class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public Users.UserResponseDto User { get; set; } = new();
}