using System;
using taggyManagement.Domain.ValueObjects;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.Events;

namespace taggyManagement.Domain.Entities
{
    public class Tag : taggyManagement.Domain.Common.AggregateRoot
    {
        public Guid Id { get; private set; }
        public string Serial { get; private set; }
        public decimal Balance { get; private set; }
        public TagStatus Status { get; private set; }

        // Domain events are managed by AggregateRoot

        public Tag(string serial, decimal initialBalance = 0m)
        {
            if (initialBalance < 0) throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative");
            Id = Guid.NewGuid();
            Serial = serial ?? throw new ArgumentNullException(nameof(serial));
            Balance = initialBalance;
            Status = TagStatus.Active;
        }

        public Result<decimal> Debit(decimal amount)
        {
            if (Status != TagStatus.Active) return Result<decimal>.Fail("Tag is not active");
            if (amount <= 0) return Result<decimal>.Fail("Amount must be positive");
            if (Balance < amount) return Result<decimal>.Fail("Insufficient balance");
            Balance -= amount;
            AddDomainEvent(new TagDebitedEvent(Id, amount, Balance));
            return Result<decimal>.Ok(Balance);
        }

        public Result<decimal> Refill(decimal amount)
        {
            if (amount <= 0) return Result<decimal>.Fail("Amount must be positive");
            Balance += amount;
            AddDomainEvent(new TagRefilledEvent(Id, amount, Balance));
            return Result<decimal>.Ok(Balance);
        }

        public Result<TagStatus> Block()
        {
            Status = TagStatus.Blocked;
            return Result<TagStatus>.Ok(Status);
        }

        public Result<TagStatus> SetMaintenance()
        {
            Status = TagStatus.Maintenance;
            return Result<TagStatus>.Ok(Status);
        }
    }
}
