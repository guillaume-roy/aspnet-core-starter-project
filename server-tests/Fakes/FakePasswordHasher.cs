using System.Threading.Tasks;

using ServerDomain.Services;

namespace ServerTests.Fakes;

public class FakePasswordHasher : IPasswordHasher
{
    public Task<string> HashPassword(string password)
    {
        return Task.FromResult(password);
    }

    public Task<bool> VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return Task.FromResult(string.Equals(hashedPassword, providedPassword));
    }
}