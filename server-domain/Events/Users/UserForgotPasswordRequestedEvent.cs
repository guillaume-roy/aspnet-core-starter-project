using ServerDomain.Kernel.Events;

namespace ServerDomain.Events.Users;

public class UserForgotPasswordRequestedEvent : DomainEvent
{
    public string Email { get; }
    public Guid Token { get; }

    public UserForgotPasswordRequestedEvent(string email, Guid token)
    {
        Email = email;
        Token = token;
    }
}