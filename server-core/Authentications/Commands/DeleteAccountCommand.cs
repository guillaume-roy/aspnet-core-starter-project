using ServerCore.Kernel.Authorizations;
using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Repositories;
using ServerCore.Services;

using ServerDomain.Entities.Users;

namespace ServerCore.Authentications.Commands;

public record DeleteAccountCommand : ICommand, IUserMustBeAuthenticated
{
}

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly IWriteRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserSession _userSession;

    public DeleteAccountCommandHandler(IWriteRepository<User> userRepository,
        IUnitOfWork unitOfWork, IUserSession userSession)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userSession = userSession;
    }

    public async Task<CommandResult> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(_ => _.Id == _userSession.UserId);
        user.Delete();
        await _unitOfWork.Commit();
        return CommandResult.Completed;
    }
}