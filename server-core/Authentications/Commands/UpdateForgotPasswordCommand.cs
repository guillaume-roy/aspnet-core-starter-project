using Microsoft.EntityFrameworkCore;

using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Exceptions;
using ServerCore.Kernel.Repositories;

using ServerDomain.Entities.Users;
using ServerDomain.Services;

namespace ServerCore.Authentications.Commands;

public record UpdateForgotPasswordCommand : ICommand
{
    public Guid Token { get; init; }
    public string Password { get; init; }
}

public class UpdateForgotPasswordCommandHandler : ICommandHandler<UpdateForgotPasswordCommand>
{
    private readonly IWriteRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateProvider _dateProvider;

    public UpdateForgotPasswordCommandHandler(IWriteRepository<User> userRepository, IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher, IDateProvider dateProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _dateProvider = dateProvider;
    }

    public async Task<CommandResult> Handle(UpdateForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(
            u => u.IsEnabled
            && u.ForgotPasswordRequests.Any(x => x.Token == request.Token && x.ExpirationDate >= _dateProvider.Now),
            new Func<IQueryable<User>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, object>>[] {
                _ => _.Include(c => c.ForgotPasswordRequests)
            });

        if (user == null)
        {
            throw new NotFoundException();
        }

        await user.SetPassword(request.Password, _passwordHasher);

        await _unitOfWork.Commit();

        return CommandResult.Completed;
    }
}