using taggyManagement.Domain.Entities;

namespace taggyManagement.Application.Services;

public interface IAuthService
{
    string CreateAccessToken(User user);
}