using Microsoft.EntityFrameworkCore;
using taggyManagement.Application.DTOs.AutoRefill;
using taggyManagement.Application.DTOs.Toll;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Domain.ValueObjects;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Services;

public sealed class AutoRefillService : IAutoRefillService
{
    private readonly IAutoRefillSettingsRepository _autoRefillSettingsRepository;
    private readonly ITagAccountRepository _tagAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly TaggyDbContext _context;

    public AutoRefillService(
        IAutoRefillSettingsRepository autoRefillSettingsRepository,
        ITagAccountRepository tagAccountRepository,
        ITransactionRepository transactionRepository,
        TaggyDbContext context)
    {
        _autoRefillSettingsRepository = autoRefillSettingsRepository;
        _tagAccountRepository = tagAccountRepository;
        _transactionRepository = transactionRepository;
        _context = context;
    }

    public async Task<Result<AutoRefillSettingsResponseDto>> GetSettingsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<AutoRefillSettingsResponseDto>.Fail("UserId is required");
        }

        var settings = await _autoRefillSettingsRepository.GetByUserIdAsync(userId, cancellationToken);
        if (settings is null)
        {
            return Result<AutoRefillSettingsResponseDto>.Fail("Auto-refill settings not found");
        }

        return Result<AutoRefillSettingsResponseDto>.Ok(ToDto(settings));
    }

    public async Task<Result<AutoRefillSettingsResponseDto>> ConfigureAsync(Guid userId, ConfigureAutoRefillRequestDto request, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<AutoRefillSettingsResponseDto>.Fail("UserId is required");
        }

        var validationResult = ValidateAutoRefillAmounts(request.MinimumBalance, request.RechargeAmount);
        if (!validationResult.IsSuccess)
        {
            return Result<AutoRefillSettingsResponseDto>.Fail(validationResult.Error!);
        }

        var settings = await _autoRefillSettingsRepository.GetByUserIdAsync(userId, cancellationToken);
        if (settings is null)
        {
            settings = AutoRefillSettings.Create(userId, request.Enabled, request.MinimumBalance, request.RechargeAmount);
            await _autoRefillSettingsRepository.AddAsync(settings, cancellationToken);
            return Result<AutoRefillSettingsResponseDto>.Ok(ToDto(settings));
        }

        settings.Configure(request.Enabled, request.MinimumBalance, request.RechargeAmount);
        await _autoRefillSettingsRepository.UpdateAsync(settings, cancellationToken);

        return Result<AutoRefillSettingsResponseDto>.Ok(ToDto(settings));
    }

    public async Task<Result<SimulateTollPassageResponseDto>> SimulateTollPassageAsync(Guid userId, SimulateTollPassageRequestDto request, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            return Result<SimulateTollPassageResponseDto>.Fail("UserId is required");
        }

        if (request.Amount <= 0)
        {
            return Result<SimulateTollPassageResponseDto>.Fail("Amount must be greater than zero");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return Result<SimulateTollPassageResponseDto>.Fail("Description is required");
        }

        var tagAccount = await _tagAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (tagAccount is null)
        {
            return Result<SimulateTollPassageResponseDto>.Fail("Tag account not found");
        }

        var settings = await _autoRefillSettingsRepository.GetByUserIdAsync(userId, cancellationToken);
        var previousBalance = tagAccount.Balance;
        var autoRefillTriggered = false;

        await using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            tagAccount.Debit(request.Amount);

            var tollTransaction = Transaction.Create(
                tagAccount.Id,
                TransactionType.TollDebit,
                request.Amount,
                request.Description);

            await _transactionRepository.AddAsync(tollTransaction, cancellationToken);

            if (settings is not null && settings.Enabled && tagAccount.Balance < settings.MinimumBalance)
            {
                tagAccount.Recharge(settings.RechargeAmount);

                var rechargeTransaction = Transaction.Create(
                    tagAccount.Id,
                    TransactionType.Recharge,
                    settings.RechargeAmount,
                    "Auto-refill recharge");

                await _transactionRepository.AddAsync(rechargeTransaction, cancellationToken);
                autoRefillTriggered = true;
            }

            await dbTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await dbTransaction.RollbackAsync(cancellationToken);
            throw;
        }

        return Result<SimulateTollPassageResponseDto>.Ok(new SimulateTollPassageResponseDto
        {
            PreviousBalance = previousBalance,
            TollAmount = request.Amount,
            CurrentBalance = tagAccount.Balance,
            AutoRefillTriggered = autoRefillTriggered
        });
    }

    private static Result ValidateAutoRefillAmounts(decimal minimumBalance, decimal rechargeAmount)
    {
        if (minimumBalance <= 0)
        {
            return Result.Fail("MinimumBalance must be greater than zero");
        }

        if (rechargeAmount <= 0)
        {
            return Result.Fail("RechargeAmount must be greater than zero");
        }

        return Result.Ok();
    }

    private static AutoRefillSettingsResponseDto ToDto(AutoRefillSettings settings)
    {
        return new AutoRefillSettingsResponseDto
        {
            Id = settings.Id,
            UserId = settings.UserId,
            Enabled = settings.Enabled,
            MinimumBalance = settings.MinimumBalance,
            RechargeAmount = settings.RechargeAmount,
            CreatedAt = settings.CreatedAt,
            UpdatedAt = settings.UpdatedAt
        };
    }
}
