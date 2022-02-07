using System;
using System.Linq;

using FluentAssertions;

using ServerCore.Authentications.Commands;
using ServerCore.Kernel.Commands;

using ServerDomain.Entities.Users;
using ServerDomain.Events.Users;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Commands;

public class RequestForgotPasswordCommandTests
{
    [Fact]
    public async void RequestForgotPasswordCommand_Should_Be_Ok()
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

        var command = new RequestForgotPasswordCommand
        {
            Email = email,
            ExpirationMinutes = 5,
        };

        var commandHandler = new RequestForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakeEntityIdGenerator(),
            new FakeDateProvider(DateTime.Now));

        await commandHandler.Handle(command, default);

        user.ForgotPasswordRequests.Should().HaveCount(1);
        user.ForgotPasswordRequests.First().ExpirationDate.Should().BeCloseTo(DateTime.Now.AddMinutes(4), TimeSpan.FromMinutes(2));
        user.ForgotPasswordRequests.First().Token.Should().NotBeEmpty();
        user.DomainEvents.Should().ContainEquivalentOf(new UserForgotPasswordRequestedEvent(user.Email, user.ForgotPasswordRequests.First().Token));
    }

    [Fact]
    public async void RequestForgotPasswordCommand_With_Invalid_Email_Should_Not_Be_Ok()
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

        var command = new RequestForgotPasswordCommand
        {
            Email = "janesmith@test.com",
            ExpirationMinutes = 5,
        };

        var commandHandler = new RequestForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakeEntityIdGenerator(),
            new FakeDateProvider(DateTime.Now));

        var commandResult = await commandHandler.Handle(command, default);
        commandResult.Should().BeOfType<CommandResult>();
    }

    [Fact]
    public async void RequestForgotPasswordCommand_With_Disabled_User_Should_Not_Be_Ok()
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

        user.IsEnabled = false;

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        userRepository.Entities.Add(user);

        var command = new RequestForgotPasswordCommand
        {
            Email = email,
            ExpirationMinutes = 5,
        };

        var commandHandler = new RequestForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakeEntityIdGenerator(),
            new FakeDateProvider(DateTime.Now));

        var commandResult = await commandHandler.Handle(command, default);
        commandResult.Should().BeOfType<CommandResult>();
    }
}