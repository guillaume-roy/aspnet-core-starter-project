using FluentAssertions;

using ServerCore.Authentications.Commands;
using ServerCore.Kernel.Commands;

using ServerDomain.Entities.Users;
using ServerDomain.Kernel.BusinessRules;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Commands;

public class UpdatePasswordCommandTests
{
    [Fact]
    public async void UpdatePassword_Should_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var oldPassword = "Johndoe06!";
        var newPassword = "Johndoe06!2";

        var user = await User.Create(
            email,
            oldPassword,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        userRepository.Entities.Add(user);

        var command = new UpdatePasswordCommand
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var commandHandler = new UpdatePasswordCommandHandler(
            new FakeUserSession(user.Id),
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher());

        var commandResult = await commandHandler.Handle(command, default);

        user.Password.Should().Be(newPassword);
    }

    [Fact]
    public async void UpdatePassword_Should_Not_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var oldPassword = "Johndoe06!";
        var newPassword = "weak";

        var user = await User.Create(
            email,
            oldPassword,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        userRepository.Entities.Add(user);

        var command = new UpdatePasswordCommand
        {
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var commandHandler = new UpdatePasswordCommandHandler(
            new FakeUserSession(user.Id),
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher());

        var action = async () => await commandHandler.Handle(command, default);

        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }
}