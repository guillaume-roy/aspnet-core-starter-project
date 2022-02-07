using ServerDomain.Entities.Users;
using ServerDomain.Kernel.BusinessRules;

namespace ServerDomain.BusinessRules.Users;

public class UserCanHaveOnlyOneRoleBusinessRule : IBusinessRule
{
    private readonly User _user;

    public UserCanHaveOnlyOneRoleBusinessRule(User user)
    {
        _user = user;
    }

    public string ErrorCode => BusinessRuleErrorCode.UserAlreadyHasOneRole;

    public Task<bool> IsBroken()
    {
        return Task.FromResult(_user.Roles.Count > 1);
    }
}