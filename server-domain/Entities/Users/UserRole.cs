using ServerDomain.Kernel.Entities;
using ServerDomain.Services;

namespace ServerDomain.Entities.Users;

public class UserRole : EntityBase
{
    public Guid UserId { get; }
    public User User { get; }
    public UserRoleTypeEnum Type { get; set; }
    public bool IsEnabled { get; set; }

    public static UserRole Create(UserRoleTypeEnum type, IEntityIdGenerator entityIdGenerator)
    {
        return new UserRole
        {
            Id = entityIdGenerator.GenerateId(),
            Type = type,
            IsEnabled = true
        };
    }
}
