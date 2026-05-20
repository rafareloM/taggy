using System;

namespace taggyManagement.Domain.Events
{
    public class TagRefilledEvent : IDomainEvent
    {
        public Guid TagId { get; }
        public decimal Amount { get; }
        public decimal BalanceAfter { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public TagRefilledEvent(Guid tagId, decimal amount, decimal balanceAfter)
        {
            TagId = tagId;
            Amount = amount;
            BalanceAfter = balanceAfter;
        }
    }
}
