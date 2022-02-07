using ServerDomain.Services;

namespace ServerInfrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public Task<string> HashPassword(string password)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword));
    }
}