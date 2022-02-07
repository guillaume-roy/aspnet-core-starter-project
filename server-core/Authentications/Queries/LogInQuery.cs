using Microsoft.EntityFrameworkCore;

using ServerCore.Kernel.Exceptions;
using ServerCore.Kernel.Queries;
using ServerCore.Kernel.Repositories;

using ServerDomain.Entities.Users;
using ServerDomain.Kernel.BusinessRules;
using ServerDomain.Services;

namespace ServerCore.Authentications.Queries;

public record LogInQuery : IQuery<LogInResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public record LogInResponse
{
    public Guid UserId { get; init; }
}

public class LogInQueryHandler : IQueryHandler<LogInQuery, LogInResponse>
{
    private readonly IReadRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LogInQueryHandler(IReadRepository<User> userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LogInResponse> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var email = User.NormalizeEmail(request.Email);
        var user = await _userRepository.Get(
            u => u.Email == email,
            new Func<IQueryable<User>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, object>>[] {
                u => u.Include(c => c.Roles)
            });

        if (user == null
             || !user.IsEnabled
             || user.Roles.All(r => !r.IsEnabled))
        {
            throw new NotFoundException();
        }

        try
        {
            await user.IsPasswordValid(request.Password, _passwordHasher);
        }
        catch (BusinessRuleValidationException)
        {
            throw new NotFoundException();
        }

        return new LogInResponse
        {
            UserId = user.Id
        };
    }
}