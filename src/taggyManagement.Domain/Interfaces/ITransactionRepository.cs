using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Domain.Entities;

namespace taggyManagement.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IReadOnlyList<Transaction>> GetByTagAccountIdAsync(Guid tagAccountId, CancellationToken cancellationToken = default);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
