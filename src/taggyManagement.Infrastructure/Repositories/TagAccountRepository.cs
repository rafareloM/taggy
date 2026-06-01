using Microsoft.EntityFrameworkCore;
using taggyManagement.Domain.Entities;
using taggyManagement.Domain.Interfaces;
using taggyManagement.Infrastructure.Data;

namespace taggyManagement.Infrastructure.Repositories;

public sealed class TagAccountRepository : ITagAccountRepository
{
    private readonly TaggyDbContext _context;

    public TagAccountRepository(TaggyDbContext context)
    {
        _context = context;
    }

    public Task<TagAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.TagAccounts.FirstOrDefaultAsync(tagAccount => tagAccount.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(TagAccount tagAccount, CancellationToken cancellationToken = default)
    {
        await _context.TagAccounts.AddAsync(tagAccount, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TagAccount tagAccount, CancellationToken cancellationToken = default)
    {
        _context.TagAccounts.Update(tagAccount);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
