using System.Linq;

using FluentAssertions;

using ServerCore.Authentications.Commands;

using ServerDomain.Entities.Users;
using ServerDomain.Events.Users;
using ServerDomain.Kernel.BusinessRules;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Core.Authentications.Commands;

public class SignUpCustomerCommandTests
{
    [Theory]
    [InlineData("JohnDoe@Test.com ", "JohnDoe06!", "johndoe@test.com")]
    [InlineData("John.Doe@Test.com ", "JohnDoe06!", "john.doe@test.com")]
    [InlineData("John_Doe1@Test.co.uk ", "JohnDoe06!", "john_doe1@test.co.uk")]
    public async void SignUpCustomerCommand_Should_Be_Ok(string originalEmail, string password, string normalizedEmail)
    {
        var command = new SignUpCustomerCommand
        {
            Email = originalEmail,
            Password = password
        };

        var repository = new FakeWriteRepository<User>();
        var commandHandler = new SignUpCustomerCommandHandler(
            repository,
            new FakeUnitOfWork(),
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        var commandResult = await commandHandler.Handle(command, default);

        repository.Entities.Should().HaveCount(1);
        repository.Entities[0].IsEnabled.Should().BeTrue();
        repository.Entities[0].Email.Should().Be(normalizedEmail);
        repository.Entities[0].Roles.Should().HaveCount(1);
        repository.Entities[0].Roles.First().IsEnabled.Should().BeTrue();
        repository.Entities[0].Roles.First().Type.Should().Be(UserRoleTypeEnum.Customer);
        repository.Entities[0].DomainEvents.Should().ContainEquivalentOf(new UserCustomerHasBeenCreatedEvent(repository.Entities[0].Email));
    }

    [Theory]
    [InlineData("JohnDoe", "JohnDoe06!")]
    [InlineData("JohnDoe@Test.com ", "JohnDoe")]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("JohnDoe@Test.com ", "")]
    [InlineData("JohnDoe@Test.com ", " ")]
    [InlineData("JohnDoe@Test.com ", null)]
    [InlineData(" ", "JohnDoe06!")]
    [InlineData("", "JohnDoe06!")]
    [InlineData(null, "JohnDoe06!")]
    public async void SignUpCustomerCommand_Should_Not_Be_Ok(string email, string password)
    {
        var command = new SignUpCustomerCommand
        {
            Email = email,
            Password = password
        };

        var repository = new FakeWriteRepository<User>();
        var commandHandler = new SignUpCustomerCommandHandler(
            repository,
            new FakeUnitOfWork(),
            new FakeUserEmailUniquenessChecker(true),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        var action = async () => await commandHandler.Handle(command, default);
        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }

    [Fact]
    public async void SignUpCustomerCommand_With_Existing_User_Should_Not_Be_Ok()
    {
        var email = "JohnDoe@Test.com";
        var password = "JohnDoe06!";

        var command = new SignUpCustomerCommand
        {
            Email = email,
            Password = password
        };

        var repository = new FakeWriteRepository<User>();
        var commandHandler = new SignUpCustomerCommandHandler(
            repository,
            new FakeUnitOfWork(),
            new FakeUserEmailUniquenessChecker(false),
            new FakeEntityIdGenerator(),
            new FakePasswordHasher());

        var action = async () => await commandHandler.Handle(command, default);

        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }
}