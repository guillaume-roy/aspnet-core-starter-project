using Microsoft.EntityFrameworkCore;

using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Repositories;

using ServerDomain.Entities.Users;
using ServerDomain.Services;

namespace ServerCore.Authentications.Commands;

public class RequestForgotPasswordCommand : ICommand
{
    public string Email { get; set; }
    public int ExpirationMinutes { get; set; }
}

public class RequestForgotPasswordCommandHandler : ICommandHandler<RequestForgotPasswordCommand>
{
    private readonly IWriteRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityIdGenerator _entityIdGenerator;
    private readonly IDateProvider _dateProvider;

    public RequestForgotPasswordCommandHandler(IWriteRepository<User> userRepository, IUnitOfWork unitOfWork,
        IEntityIdGenerator entityIdGenerator, IDateProvider dateProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _entityIdGenerator = entityIdGenerator;
        _dateProvider = dateProvider;
    }

    public async Task<CommandResult> Handle(RequestForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var email = User.NormalizeEmail(request.Email);
        var user = await _userRepository.Get(
            u => u.Email == email && u.IsEnabled,
            new Func<IQueryable<User>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, object>>[] {
                _ => _.Include(c => c.ForgotPasswordRequests)
            });

        if (user == null)
        {
            return CommandResult.Completed;
        }

        user.RequestForgotPassword(_dateProvider, request.ExpirationMinutes, _entityIdGenerator);

        await _unitOfWork.Commit();

        return CommandResult.Completed;
    }
}