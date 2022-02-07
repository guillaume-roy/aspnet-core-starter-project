using System;

using FluentAssertions;

using ServerCore.Authentications.Queries;
using ServerCore.Kernel.Exceptions;

using ServerDomain.Entities.Users;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Queries;

public class LogInQueryTests
{
    [Theory]
    [InlineData("JohnDoe@Test.com ", "JohnDoe06!")]
    public async void LogInQuery_Should_Be_Ok(string email, string password)
    {
        var readRepository = new FakeReadRepository<User>();

        var user = await User.Create(
            email,
            password,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        readRepository.Entities.Add(user);

        var query = new LogInQuery
        {
            Email = email,
            Password = password
        };

        var queryHandler = new LogInQueryHandler(
            readRepository,
            new FakePasswordHasher());

        var queryResult = await queryHandler.Handle(query, default);
        queryResult.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async void LogInQuery_With_None_Existing_User_Should_Not_Be_Ok()
    {
        var query = new LogInQuery
        {
            Email = "test",
            Password = "test"
        };

        var queryHandler = new LogInQueryHandler(
            new FakeReadRepository<User>(),
            new FakePasswordHasher());

        var action = async () => await queryHandler.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void LogInQuery_With_Disabled_User_Should_Not_Be_Ok()
    {
        var readRepository = new FakeReadRepository<User>();

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

        readRepository.Entities.Add(user);

        var query = new LogInQuery
        {
            Email = email,
            Password = password
        };

        var queryHandler = new LogInQueryHandler(
            readRepository,
            new FakePasswordHasher());

        var action = async () => await queryHandler.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void LogInQuery_With_Disabled_Role_Should_Not_Be_Ok()
    {
        var readRepository = new FakeReadRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";

        var user = await User.Create(
            email,
            password,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        user.Role.IsEnabled = false;

        readRepository.Entities.Add(user);

        var query = new LogInQuery
        {
            Email = email,
            Password = password
        };

        var queryHandler = new LogInQueryHandler(
            readRepository,
            new FakePasswordHasher());

        var action = async () => await queryHandler.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void LogInQuery_With_Invalid_Email_Should_Not_Be_Ok()
    {
        var readRepository = new FakeReadRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";

        var user = await User.Create(
            email,
            password,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        readRepository.Entities.Add(user);

        var query = new LogInQuery
        {
            Email = "janesmith@test.com",
            Password = password
        };

        var queryHandler = new LogInQueryHandler(
            readRepository,
            new FakePasswordHasher());

        var action = async () => await queryHandler.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async void LogInQuery_With_Invalid_Password_Should_Not_Be_Ok()
    {
        var readRepository = new FakeReadRepository<User>();

        var email = "Johndoe@test.com";
        var password = "Johndoe06!";

        var user = await User.Create(
            email,
            password,
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        await user.AddCustomerRole(new FakeEntityIdGenerator());

        readRepository.Entities.Add(user);

        var query = new LogInQuery
        {
            Email = email,
            Password = "123456"
        };

        var queryHandler = new LogInQueryHandler(
            readRepository,
            new FakePasswordHasher());

        var action = async () => await queryHandler.Handle(query, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }
}