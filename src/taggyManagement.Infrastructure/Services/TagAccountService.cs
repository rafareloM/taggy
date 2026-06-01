using taggyManagement.Application.DTOs.TagAccounts;
using taggyManagement.Application.Services;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Infrastructure.Services;

public sealed class TagAccountService : ITagAccountService
{
    private readonly ITagAccountRepository _tagAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TagAccountService(ITagAccountRepository tagAccountRepository, ITransactionRepository transactionRepository)
    {
        _tagAccountRepository = tagAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<TagBalanceResponseDto>> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tagAccount = await _tagAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (tagAccount is null)
        {
            return Result<TagBalanceResponseDto>.Fail("Tag account not found");
        }

        return Result<TagBalanceResponseDto>.Ok(ToBalanceDto(tagAccount));
    }

    public async Task<Result<TagBalanceResponseDto>> RechargeAsync(Guid userId, RechargeRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
        {
            return Result<TagBalanceResponseDto>.Fail("Amount must be greater than zero");
        }

        var tagAccount = await _tagAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (tagAccount is null)
        {
            return Result<TagBalanceResponseDto>.Fail("Tag account not found");
        }

        tagAccount.Recharge(request.Amount);

        var transaction = Transaction.Create(
            tagAccount.Id,
            TransactionType.Recharge,
            request.Amount,
            "Tag account recharge");

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return Result<TagBalanceResponseDto>.Ok(ToBalanceDto(tagAccount));
    }

    public async Task<Result<IReadOnlyList<TransactionResponseDto>>> GetStatementAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tagAccount = await _tagAccountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (tagAccount is null)
        {
            return Result<IReadOnlyList<TransactionResponseDto>>.Fail("Tag account not found");
        }

        var transactions = await _transactionRepository.GetByTagAccountIdAsync(tagAccount.Id, cancellationToken);
        return Result<IReadOnlyList<TransactionResponseDto>>.Ok(transactions.Select(ToDto).ToList());
    }

    private static TagBalanceResponseDto ToBalanceDto(TagAccount tagAccount)
    {
        return new TagBalanceResponseDto
        {
            Balance = tagAccount.Balance
        };
    }

    private static TransactionResponseDto ToDto(Transaction transaction)
    {
        return new TransactionResponseDto
        {
            Id = transaction.Id,
            TagAccountId = transaction.TagAccountId,
            Type = transaction.Type,
            Amount = transaction.Amount,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}
