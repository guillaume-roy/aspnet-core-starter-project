using FluentAssertions;

using ServerCore.Services;

using ServerDomain.Entities.Users;
using ServerDomain.Kernel.BusinessRules;

using ServerTests.Fakes;

using Xunit;

namespace ServerTests.Domain.Entities.Users;

public class UserTests
{
    [Fact]
    public async void Create_Ok()
    {
        var email = "johndoe@test.com";
        var password = "123456Aa!";

        var userEmailUniquenessCheckerService = new FakeUserEmailUniquenessChecker(true);
        var entityIdGeneratorService = new FakeEntityIdGenerator();
        var passwordHasher = new FakePasswordHasher();

        var user = await User.Create(email, password, userEmailUniquenessCheckerService, entityIdGeneratorService, passwordHasher);

        user.Email.Should().Be(email);
    }

    [Fact]
    public async void Create_With_Invalid_Email()
    {
        var email = "johndoe";
        var password = "123456Aa!";

        var userEmailUniquenessCheckerService = new FakeUserEmailUniquenessChecker(true);
        var entityIdGeneratorService = new FakeEntityIdGenerator();
        var passwordHasher = new FakePasswordHasher();

        var action = async () => await User.Create(email, password, userEmailUniquenessCheckerService, entityIdGeneratorService, passwordHasher);

        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }

    [Fact]
    public async void Create_With_Existing_Email()
    {
        var email = "johndoe";
        var password = "123456Aa!";

        var userEmailUniquenessCheckerService = new FakeUserEmailUniquenessChecker(false);
        var entityIdGeneratorService = new FakeEntityIdGenerator();
        var passwordHasher = new FakePasswordHasher();

        var action = async () => await User.Create(email, password, userEmailUniquenessCheckerService, entityIdGeneratorService, passwordHasher);

        await action.Should().ThrowAsync<BusinessRuleValidationException>();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(" ", "")]
    [InlineData("John_Doe1@test.COM", "john_doe1@test.com")]
    [InlineData(" JohnDoe1@test.COM", "johndoe1@test.com")]
    [InlineData("JohnDoe1@test.COM ", "johndoe1@test.com")]
    public async void NormalizeEmail_Should_Be_Ok(string input, string output)
    {
        User.NormalizeEmail(input).Should().Be(output);
    }
}