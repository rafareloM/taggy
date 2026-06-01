using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using taggyManagement.Application.DTOs.TagAccounts;
using taggyManagement.Domain.Common;

namespace taggyManagement.Application.Services;

public interface ITagAccountService
{
    Task<Result<TagBalanceResponseDto>> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<TagBalanceResponseDto>> RechargeAsync(Guid userId, RechargeRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TransactionResponseDto>>> GetStatementAsync(Guid userId, CancellationToken cancellationToken = default);
}
