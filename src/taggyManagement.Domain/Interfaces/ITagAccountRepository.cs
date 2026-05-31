using System;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface ITagAccountRepository
{
    Task<TagAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(TagAccount tagAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(TagAccount tagAccount, CancellationToken cancellationToken = default);
}
