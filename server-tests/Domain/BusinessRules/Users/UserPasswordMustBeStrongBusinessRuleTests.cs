using FluentAssertions;

using ServerDomain.BusinessRules.Users;
using ServerDomain.Kernel.BusinessRules;

using Xunit;

namespace ServerTests.Domain.BusinessRules.Users;

public class UserPasswordMustBeStrongBusinessRuleTests
{
    [Theory]
    [InlineData("123456Aa!")]
    [InlineData("*123456Aa!")]
    [InlineData("\"123456Aa!$")]
    [InlineData("(123456Aa!$ ")]
    [InlineData("+123456Aa!$")]
    [InlineData("}123456Aa!$.")]
    [InlineData("1 Lorem ipsum dolor sit amet, consectetur adipiscing elit pos")]
    public async void Check_Ok(string password)
    {
        var userPasswordMustBeStrongBusinessRule = new UserPasswordMustBeStrongBusinessRule(password);
        var isBoken = await userPasswordMustBeStrongBusinessRule.IsBroken();
        isBoken.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("12")]
    [InlineData("aasdfraaaaa")]
    [InlineData("aasdfraaa1aa")]
    [InlineData("RRRRRRRRRRRR")]
    [InlineData("RRRRRRRRRRsRR")]
    [InlineData("111111111111111")]
    [InlineData("111111111z111111")]
    [InlineData("$%^&*()_+")]
    [InlineData("1234Aa!")]
    [InlineData("1 Lorem ipsum dolor sit amet ! Consectetur adipiscing elit posera")]
    public async void Check_Ko(string password)
    {
        var userPasswordMustBeStrongBusinessRule = new UserPasswordMustBeStrongBusinessRule(password);
        var isBoken = await userPasswordMustBeStrongBusinessRule.IsBroken();
        isBoken.Should().BeTrue();
    }
}