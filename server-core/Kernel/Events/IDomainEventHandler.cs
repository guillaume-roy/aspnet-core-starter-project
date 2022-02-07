using MediatR;

using ServerDomain.Kernel.Events;

namespace ServerCore.Kernel.Events;
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : DomainEvent
{
}