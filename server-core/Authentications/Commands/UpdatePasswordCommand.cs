using Microsoft.EntityFrameworkCore;

using ServerCore.Kernel.Authorizations;
using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Repositories;
using ServerCore.Services;

using ServerDomain.Entities.Users;
using ServerDomain.Services;

namespace ServerCore.Authentications.Commands;

public record UpdatePasswordCommand : ICommand, IUserMustBeAuthenticated
{
    public string OldPassword { get; init; }
    public string NewPassword { get; init; }
}

public class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand>
{
    private readonly IUserSession _userSession;
    private readonly IWriteRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UpdatePasswordCommandHandler(IUserSession userSession, IWriteRepository<User> userRepository, IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userSession = userSession;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<CommandResult> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(_ => _.Id == _userSession.UserId,
            new Func<IQueryable<User>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, object>>[] {
                _ => _.Include(c => c.ForgotPasswordRequests)
            });

        await user.UpdatePassword(request.OldPassword, request.NewPassword, _passwordHasher);

        await _unitOfWork.Commit();
        return CommandResult.Completed;
    }
}