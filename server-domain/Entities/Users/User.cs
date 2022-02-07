using ServerDomain.BusinessRules.Users;
using ServerDomain.Events.Users;
using ServerDomain.Kernel.Entities;
using ServerDomain.Services;

namespace ServerDomain.Entities.Users;
public class User : EntityBase, IAggregateRoot
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<UserRole> Roles { get; } = new List<UserRole>();
    public ICollection<UserForgotPasswordRequest> ForgotPasswordRequests { get; } = new List<UserForgotPasswordRequest>();
    public UserRole? Role => Roles.FirstOrDefault();

    public static async Task<User> Create(
        string email,
        string password,
        IUserEmailUniquenessChecker userEmailUniquenessChecker,
        IEntityIdGenerator entityIdGenerator,
        IPasswordHasher passwordHasher)
    {
        email = NormalizeEmail(email);

        await CheckRule(new UserEmailMustBeValidBusinessRule(email));
        await CheckRule(new UserEmailMustBeUniqueBusinessRule(email, userEmailUniquenessChecker));

        var user = new User
        {
            Id = entityIdGenerator.GenerateId(),
            Email = NormalizeEmail(email),
            IsEnabled = true,
        };

        await user.SetPassword(password, passwordHasher);

        return user;
    }

    public void Delete()
    {
        Email = $"DELETED_{Guid.NewGuid()}";
        Password = $"DELETED_{Guid.NewGuid()}";
        IsEnabled = false;
        AddDomainEvent(new UserHasBeenDeletedEvent(Id));
    }

    public void RequestForgotPassword(IDateProvider dateProvider, int expirationMinutes, IEntityIdGenerator entityIdGenerator)
    {
        var request = UserForgotPasswordRequest.Create(dateProvider, expirationMinutes, entityIdGenerator);
        ForgotPasswordRequests.Add(request);
        AddDomainEvent(new UserForgotPasswordRequestedEvent(Email, request.Token));
    }

    public static string NormalizeEmail(string email)
    {
        return string.IsNullOrEmpty(email)
            ? string.Empty
            : email.ToLowerInvariant().Trim();
    }

    public async Task UpdatePassword(string oldPassword, string newPassword, IPasswordHasher passwordHasher)
    {
        await CheckRule(new UserPasswordCheckBusinessRule(Password, oldPassword, passwordHasher));
        await SetPassword(newPassword, passwordHasher);
    }

    public async Task SetPassword(string password, IPasswordHasher passwordHasher)
    {
        await CheckRule(new UserPasswordMustBeStrongBusinessRule(password));
        Password = await passwordHasher.HashPassword(password);
        ForgotPasswordRequests.Clear();
    }

    public async Task<bool> IsPasswordValid(string password, IPasswordHasher passwordHasher)
    {
        await CheckRule(new UserPasswordCheckBusinessRule(Password, password, passwordHasher));
        return true;
    }

    public async Task AddCustomerRole(IEntityIdGenerator entityIdGenerator)
    {
        await CheckRule(new UserCanHaveOnlyOneRoleBusinessRule(this));
        Roles.Add(UserRole.Create(UserRoleTypeEnum.Customer, entityIdGenerator));
        AddDomainEvent(new UserCustomerHasBeenCreatedEvent(Email));
    }

    public bool IsAdmin()
    {
        return Role?.Type == UserRoleTypeEnum.Admin && Role?.IsEnabled == true;
    }

    public bool IsOwner(Guid shopId)
    {
        return Role?.Type == UserRoleTypeEnum.ShopOwner && Role?.IsEnabled == true; // && Role.ShopId == shopId;
    }

    public bool IsCustomer()
    {
        return Role?.Type == UserRoleTypeEnum.Customer && Role?.IsEnabled == true;
    }
}
