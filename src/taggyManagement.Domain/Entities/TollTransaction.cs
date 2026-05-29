using System;

namespace taggyManagement.Domain.Entities
{
    public class TollTransaction
    {
        public Guid Id { get; private set; }
        public Guid TripId { get; private set; }
        public Guid TagId { get; private set; }
        public Guid TollPlazaId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime OccurredAt { get; private set; }

        public TollTransaction(Guid tripId, Guid tagId, Guid tollPlazaId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Id = Guid.NewGuid();
            TripId = tripId;
            TagId = tagId;
            TollPlazaId = tollPlazaId;
            Amount = amount;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
