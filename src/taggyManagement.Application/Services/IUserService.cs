using System;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Application.DTOs.Auth;
using taggyManagement.Application.DTOs.Users;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface IUserService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<UserResponseDto>> GetMeAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<UserResponseDto>> UpdateMeAsync(Guid userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteMeAsync(Guid userId, CancellationToken cancellationToken = default);
}