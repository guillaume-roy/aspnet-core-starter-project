using System.Text.RegularExpressions;

using ServerDomain.Kernel.BusinessRules;

namespace ServerDomain.BusinessRules.Users;

public class UserPasswordMustBeStrongBusinessRule : IBusinessRule
{
    private readonly string _password;

    public string ErrorCode => BusinessRuleErrorCode.UserPasswordIsTooWeak;

    public UserPasswordMustBeStrongBusinessRule(string password)
    {
        _password = password;
    }

    public Task<bool> IsBroken()
    {
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSpecialChar = new Regex(@"[ !""#$%&'()*+,-./:;<=>?@[\]^_`{|}~]+");

        var isValid = !string.IsNullOrWhiteSpace(_password)
            && _password.Length >= 8 && _password.Length <= 64
            && hasNumber.IsMatch(_password)
            && hasUpperChar.IsMatch(_password)
            && hasLowerChar.IsMatch(_password)
            && hasSpecialChar.IsMatch(_password);

        return Task.FromResult(!isValid);
    }
}