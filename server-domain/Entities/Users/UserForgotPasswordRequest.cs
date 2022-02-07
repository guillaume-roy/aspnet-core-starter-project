using ServerDomain.Kernel.Entities;
using ServerDomain.Services;

namespace ServerDomain.Entities.Users;

public class UserForgotPasswordRequest : EntityBase
{
    public Guid UserId { get; }
    public User User { get; }
    public Guid Token { get; set; }
    public DateTime ExpirationDate { get; set; }

    public static UserForgotPasswordRequest Create(IDateProvider dateProvider, int expirationMinutes, IEntityIdGenerator entityIdGenerator)
    {
        return new UserForgotPasswordRequest
        {
            Id = entityIdGenerator.GenerateId(),
            ExpirationDate = dateProvider.Now.AddMinutes(expirationMinutes),
            Token = Guid.NewGuid(),
        };
    }
}