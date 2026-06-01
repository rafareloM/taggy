using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly TaggyDbContext _context;

    public TransactionRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Transaction>> GetByTagAccountIdAsync(Guid tagAccountId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(transaction => transaction.TagAccountId == tagAccountId)
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
