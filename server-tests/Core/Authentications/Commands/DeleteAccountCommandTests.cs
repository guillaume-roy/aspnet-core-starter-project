using FluentAssertions;

using ServerCore.Authentications.Commands;

using ServerDomain.Entities.Users;
using ServerDomain.Events.Users;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Commands;

public class DeleteAccountCommandTests
{
    [Fact]
    public async void DeleteAccountCommand_Should_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";

        var user = await User.Create(
            email,
            password,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        userRepository.Entities.Add(user);

        var command = new DeleteAccountCommand();
        var commandHandler = new DeleteAccountCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakeUserSession(user.Id));

        await commandHandler.Handle(command, default);

        user.IsEnabled.Should().BeFalse();
        user.Password.Should().StartWith("DELETED_");
        user.Email.Should().StartWith("DELETED_");
        User.NormalizeEmail(user.Email).Should().NotContain(User.NormalizeEmail(email));
        user.DomainEvents.Should().ContainEquivalentOf(new UserHasBeenDeletedEvent(user.Id));
    }
}