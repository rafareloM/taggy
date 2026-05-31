using System;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Application.DTOs.TagAccounts;

public sealed class TransactionResponseDto
{
    public Guid Id { get; set; }
    public Guid TagAccountId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
