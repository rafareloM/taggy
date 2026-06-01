using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid TagAccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Transaction()
    {
    }

    public static Transaction Create(Guid tagAccountId, TransactionType type, decimal amount, string description)
    {
        if (tagAccountId == Guid.Empty) throw new ArgumentException("Tag account id is required", nameof(tagAccountId));
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount must be greater than zero");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));

        return new Transaction
        {
            Id = Guid.NewGuid(),
            TagAccountId = tagAccountId,
            Type = type,
            Amount = amount,
            Description = description.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }
}
