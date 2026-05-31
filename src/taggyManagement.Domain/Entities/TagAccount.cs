namespace taggyManagement.Domain.Entities;

public class TagAccount
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private TagAccount()
    {
    }

    public static TagAccount Create(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User id is required", nameof(userId));

        var now = DateTime.UtcNow;
        return new TagAccount
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Balance = 0,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void Recharge(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Recharge amount must be greater than zero");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Debit amount must be greater than zero");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
