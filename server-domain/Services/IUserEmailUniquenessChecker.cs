namespace ServerDomain.Services;

public interface IUserEmailUniquenessChecker
{
    Task<bool> IsEmailUnique(string email);
}
