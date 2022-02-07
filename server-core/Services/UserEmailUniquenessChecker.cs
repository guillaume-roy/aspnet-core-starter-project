using ServerCore.Kernel.Repositories;

using ServerDomain.Entities.Users;
using ServerDomain.Services;

namespace ServerCore.Services;

public class UserEmailUniquenessChecker : IUserEmailUniquenessChecker
{
    private readonly IReadRepository<User> _userRepository;

    public UserEmailUniquenessChecker(IReadRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> IsEmailUnique(string email)
    {
        return _userRepository.NotExists(u => u.Email == email);
    }
}