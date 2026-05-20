using System;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Domain.Entities
{
    public class TollTariff
    {
        public Guid Id { get; private set; }
        public Guid TollPlazaId { get; private set; }
        public PropulsionType PropulsionType { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime EffectiveFrom { get; private set; }
        public DateTime? EffectiveTo { get; private set; }

        public TollTariff(Guid tollPlazaId, PropulsionType propulsionType, decimal amount, DateTime effectiveFrom)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Id = Guid.NewGuid();
            TollPlazaId = tollPlazaId;
            PropulsionType = propulsionType;
            Amount = amount;
            EffectiveFrom = effectiveFrom;
        }

        public void EndValidity(DateTime effectiveTo)
        {
            if (effectiveTo < EffectiveFrom) throw new ArgumentOutOfRangeException(nameof(effectiveTo));
            EffectiveTo = effectiveTo;
        }
    }
}
