using ServerDomain.Kernel.Events;

namespace ServerDomain.Events.Users
{
  public class UserCustomerHasBeenCreatedEvent : DomainEvent
  {
    public string Email { get; }

    public UserCustomerHasBeenCreatedEvent(string email)
    {
      Email = email;
    }
  }
}