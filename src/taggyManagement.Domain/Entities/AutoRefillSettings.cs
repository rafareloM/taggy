namespace taggyManagement.Domain.Entities;

public class AutoRefillSettings
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public bool Enabled { get; private set; }
    public decimal MinimumBalance { get; private set; }
    public decimal RechargeAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private AutoRefillSettings()
    {
    }

    public static AutoRefillSettings Create(Guid userId, bool enabled, decimal minimumBalance, decimal rechargeAmount)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User id is required", nameof(userId));
        ValidateAmounts(minimumBalance, rechargeAmount);

        var now = DateTime.UtcNow;
        return new AutoRefillSettings
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Enabled = enabled,
            MinimumBalance = minimumBalance,
            RechargeAmount = rechargeAmount,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void Configure(bool enabled, decimal minimumBalance, decimal rechargeAmount)
    {
        ValidateAmounts(minimumBalance, rechargeAmount);

        Enabled = enabled;
        MinimumBalance = minimumBalance;
        RechargeAmount = rechargeAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateAmounts(decimal minimumBalance, decimal rechargeAmount)
    {
        if (minimumBalance <= 0) throw new ArgumentOutOfRangeException(nameof(minimumBalance), "Minimum balance must be greater than zero");
        if (rechargeAmount <= 0) throw new ArgumentOutOfRangeException(nameof(rechargeAmount), "Recharge amount must be greater than zero");
    }
}
