using System.Linq.Expressions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Queries;
using ServerCore.Kernel.Repositories;
using ServerCore.Services;

using ServerDomain.Entities.Users;

namespace ServerCore.Kernel.Authorizations;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IUserSession _userSession;
    private readonly IReadRepository<User> _userRepository;

    public AuthorizationBehavior(IUserSession userSession, IReadRepository<User> userRepository)
    {
        _userSession = userSession;
        _userRepository = userRepository;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is IUserMustBeAuthenticated
            || request is IUserMustBeAdmin
            || request is IUserMustBeOwner
            || request is IUserMustBeCustomer)
        {
            var user = await _userRepository.Get(
                _ => _.Id == _userSession.UserId && _.IsEnabled,
                new Func<IQueryable<User>, IIncludableQueryable<User, object>>[]
                {
                    _ => _.Include(x => x.Roles)
                });

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (request is IUserMustBeAdmin)
            {
                if (!user.IsAdmin())
                {
                    throw new ForbiddenAccessException();
                }
            }

            if (request is IUserMustBeOwner ownerRequest)
            {
                // TODO : Adapt
                if (!user.IsOwner(Guid.NewGuid()))
                {
                    throw new ForbiddenAccessException();
                }
            }

            if (request is IUserMustBeCustomer)
            {
                if (!user.IsCustomer())
                {
                    throw new ForbiddenAccessException();
                }
            }
        }

        return await next();
    }
}
