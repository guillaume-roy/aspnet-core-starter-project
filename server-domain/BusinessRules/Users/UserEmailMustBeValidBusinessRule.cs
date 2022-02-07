using System.Text.RegularExpressions;

using ServerDomain.Kernel.BusinessRules;

namespace ServerDomain.BusinessRules.Users;

public class UserEmailMustBeValidBusinessRule : IBusinessRule
{
    private readonly string _email;

    public string ErrorCode => BusinessRuleErrorCode.UserEmailIsInvalid;

    public UserEmailMustBeValidBusinessRule(string email)
    {
        _email = email;
    }

    public Task<bool> IsBroken()
    {
        return Task.FromResult(!Regex.IsMatch(_email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"));
    }
}