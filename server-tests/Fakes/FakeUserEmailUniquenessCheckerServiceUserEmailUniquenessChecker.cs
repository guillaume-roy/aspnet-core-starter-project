using System.Threading.Tasks;

using ServerDomain.Services;

namespace ServerTests.Fakes;

public class FakeUserEmailUniquenessChecker : IUserEmailUniquenessChecker
{
    private readonly bool _isUnique;

    public FakeUserEmailUniquenessChecker(bool isUnique)
    {
        _isUnique = isUnique;
    }

    public Task<bool> IsEmailUnique(string email)
    {
        return Task.FromResult(_isUnique);
    }
}