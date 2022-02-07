using MediatR;

namespace ServerDomain.Kernel.Events;

public abstract class DomainEvent : INotification
{
    public bool IsPublished { get; set; }
}