using taggyManagement.Application.DTOs.Auth;
using taggyManagement.Application.DTOs.Users;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;

namespace taggyManagement.Infrastructure.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITagAccountRepository _tagAccountRepository;
    private readonly IAuthService _authService;
    private readonly PasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, ITagAccountRepository tagAccountRepository, IAuthService authService, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tagAccountRepository = tagAccountRepository;
        _authService = authService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return Result<AuthResponseDto>.Fail("A user with this email already exists");
        }

        var (hash, salt) = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.FullName, request.Email, hash, salt);

        await _userRepository.AddAsync(user, cancellationToken);
        await _tagAccountRepository.AddAsync(TagAccount.Create(user.Id), cancellationToken);

        return Result<AuthResponseDto>.Ok(new AuthResponseDto
        {
            AccessToken = _authService.CreateAccessToken(user),
            User = ToDto(user)
        });
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result<AuthResponseDto>.Fail("Invalid email or password");
        }

        var isValidPassword = _passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt);
        if (!isValidPassword)
        {
            return Result<AuthResponseDto>.Fail("Invalid email or password");
        }

        return Result<AuthResponseDto>.Ok(new AuthResponseDto
        {
            AccessToken = _authService.CreateAccessToken(user),
            User = ToDto(user)
        });
    }

    public async Task<Result<UserResponseDto>> GetMeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result<UserResponseDto>.Fail("User not found");
        }

        return Result<UserResponseDto>.Ok(ToDto(user));
    }

    public async Task<Result<UserResponseDto>> UpdateMeAsync(Guid userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result<UserResponseDto>.Fail("User not found");
        }

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null && existingUser.Id != user.Id)
        {
            return Result<UserResponseDto>.Fail("A user with this email already exists");
        }

        user.UpdateProfile(request.FullName, request.Email);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<UserResponseDto>.Ok(ToDto(user));
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result.Fail("User not found");
        }

        var isValidPassword = _passwordHasher.Verify(request.CurrentPassword, user.PasswordHash, user.PasswordSalt);
        if (!isValidPassword)
        {
            return Result.Fail("Current password is invalid");
        }

        var (hash, salt) = _passwordHasher.Hash(request.NewPassword);
        user.UpdatePassword(hash, salt);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeleteMeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result.Fail("User not found");
        }

        user.Delete();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Ok();
    }

    private static UserResponseDto ToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
