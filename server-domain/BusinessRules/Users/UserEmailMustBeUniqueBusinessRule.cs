using ServerDomain.Kernel.BusinessRules;
using ServerDomain.Services;

namespace ServerDomain.BusinessRules.Users;

public class UserEmailMustBeUniqueBusinessRule : IBusinessRule
{
    private readonly string _email;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessCheckerService;

    public string ErrorCode => BusinessRuleErrorCode.UserEmailAlreadyExists;

    public UserEmailMustBeUniqueBusinessRule(string email, IUserEmailUniquenessChecker userEmailUniquenessCheckerService)
    {
        _email = email;
        _userEmailUniquenessCheckerService = userEmailUniquenessCheckerService;
    }

    public async Task<bool> IsBroken()
    {
        return !await _userEmailUniquenessCheckerService.IsEmailUnique(_email);
    }
}