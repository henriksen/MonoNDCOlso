using MediatR;

namespace Library.SharedKernel.Domain;

public class Entity
{
    private readonly List<INotification> _domainEvents = [];
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;
    protected void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}