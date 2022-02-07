using System;
using System.Linq;

using FluentAssertions;

using ServerCore.Authentications.Commands;
using ServerCore.Kernel.Exceptions;

using ServerDomain.Entities.Users;
using ServerDomain.Kernel.BusinessRules;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Commands;

public class UpdateForgotPasswordCommandTests
{
    [Fact]
    public async void UpdateForgotPasswordCommand_Should_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";
        var newPassword = "Johndoe06!2";

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

        var updateForgotPasswordCommand = new UpdateForgotPasswordCommand
        {
            Token = user.ForgotPasswordRequests.First().Token,
            Password = newPassword,
        };

        var updateForgotPasswordCommandHandler = new UpdateForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher(),
            new FakeDateProvider(DateTime.Now));

        await updateForgotPasswordCommandHandler.Handle(updateForgotPasswordCommand, default);

        user.ForgotPasswordRequests.Should().BeEmpty();
        user.Password.Should().Be(newPassword);
    }

    [Fact]
    public async void UpdateForgotPasswordCommand_With_Invalid_Token_Should_Not_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";
        var newPassword = "Johndoe06!2";

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

        var updateForgotPasswordCommand = new UpdateForgotPasswordCommand
        {
            Token = Guid.NewGuid(),
            Password = newPassword,
        };

        var updateForgotPasswordCommandHandler = new UpdateForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher(),
            new FakeDateProvider(DateTime.Now));

        var action = async () => await updateForgotPasswordCommandHandler.Handle(updateForgotPasswordCommand, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void UpdateForgotPasswordCommand_With_Disabled_User_Should_Not_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";
        var newPassword = "Johndoe06!2";

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

        user.IsEnabled = false;

        var updateForgotPasswordCommand = new UpdateForgotPasswordCommand
        {
            Token = user.ForgotPasswordRequests.First().Token,
            Password = newPassword,
        };

        var updateForgotPasswordCommandHandler = new UpdateForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher(),
            new FakeDateProvider(DateTime.Now));

        var action = async () => await updateForgotPasswordCommandHandler.Handle(updateForgotPasswordCommand, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void UpdateForgotPasswordCommand_With_Token_Expired_Should_Not_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";
        var newPassword = "Johndoe06!2";

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

        var updateForgotPasswordCommand = new UpdateForgotPasswordCommand
        {
            Token = user.ForgotPasswordRequests.First().Token,
            Password = newPassword,
        };

        var updateForgotPasswordCommandHandler = new UpdateForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher(),
            new FakeDateProvider(DateTime.Now.AddDays(1)));

        var action = async () => await updateForgotPasswordCommandHandler.Handle(updateForgotPasswordCommand, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void UpdateForgotPasswordCommand_With_Invalid_Password_Should_Not_Be_Ok()
    {
        var userRepository = new FakeWriteRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";
        var newPassword = "test";

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

        var updateForgotPasswordCommand = new UpdateForgotPasswordCommand
        {
            Token = user.ForgotPasswordRequests.First().Token,
            Password = newPassword,
        };

        var updateForgotPasswordCommandHandler = new UpdateForgotPasswordCommandHandler(
            userRepository,
            new FakeUnitOfWork(),
            new FakePasswordHasher(),
            new FakeDateProvider(DateTime.Now));

        var action = async () => await updateForgotPasswordCommandHandler.Handle(updateForgotPasswordCommand, default);
        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }
}