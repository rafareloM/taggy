using System.Collections.Generic;
using taggyManagement.Domain.Events;

namespace taggyManagement.Domain.Common
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        protected void AddDomainEvent(IDomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
