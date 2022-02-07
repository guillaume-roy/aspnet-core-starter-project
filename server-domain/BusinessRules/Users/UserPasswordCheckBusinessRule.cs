using ServerDomain.Kernel.BusinessRules;
using ServerDomain.Services;

namespace ServerDomain.BusinessRules.Users;

public class UserPasswordCheckBusinessRule : IBusinessRule
{
    private readonly string _userPassword;
    private readonly string _password;
    private readonly IPasswordHasher _passwordHasher;

    public string ErrorCode => BusinessRuleErrorCode.UserPasswordIsInvalid;

    public UserPasswordCheckBusinessRule(string userPassword, string password, IPasswordHasher passwordHasher)
    {
        _userPassword = userPassword;
        _password = password;
        _passwordHasher = passwordHasher;
    }

    public Task<bool> IsBroken()
    {
        return _passwordHasher.VerifyHashedPassword(_userPassword, _password)
            .ContinueWith(t => !t.Result);
    }
}