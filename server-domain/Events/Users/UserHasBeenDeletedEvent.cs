using ServerDomain.Kernel.Events;

namespace ServerDomain.Events.Users;

public class UserHasBeenDeletedEvent : DomainEvent
{
  public Guid UserId { get; }

  public UserHasBeenDeletedEvent(Guid userId)
  {
    UserId = userId;
  }
}