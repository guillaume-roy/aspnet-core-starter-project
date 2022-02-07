namespace ServerDomain.Services;

public interface IPasswordHasher
{
    Task<string> HashPassword(string password);
    Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword);
}