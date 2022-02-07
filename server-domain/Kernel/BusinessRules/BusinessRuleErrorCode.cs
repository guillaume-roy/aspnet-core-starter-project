namespace ServerDomain.Kernel.BusinessRules;

public static class BusinessRuleErrorCode
{
    public static string UserEmailIsInvalid => nameof(UserEmailIsInvalid);
    public static string UserEmailAlreadyExists => nameof(UserEmailAlreadyExists);
    public static string UserPasswordIsTooWeak => nameof(UserPasswordIsTooWeak);
    public static string UserAlreadyHasOneRole => nameof(UserAlreadyHasOneRole);
    public static string UserRefreshTokenAlreadyExists => nameof(UserRefreshTokenAlreadyExists);
    public static string UserPasswordIsInvalid => nameof(UserPasswordIsInvalid);
}