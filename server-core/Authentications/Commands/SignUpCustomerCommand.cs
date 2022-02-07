using MediatR;

using ServerCore.Kernel.Commands;
using ServerCore.Kernel.Repositories;

using ServerDomain.Entities.Users;
using ServerDomain.Services;

namespace ServerCore.Authentications.Commands;

public record SignUpCustomerCommand : ICommand
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class SignUpCustomerCommandHandler : ICommandHandler<SignUpCustomerCommand>
{
    private readonly IWriteRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly IEntityIdGenerator _entityIdGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public SignUpCustomerCommandHandler(
        IWriteRepository<User> repository,
        IUnitOfWork unitOfWork,
        IUserEmailUniquenessChecker userEmailUniquenessChecker,
        IEntityIdGenerator entityIdGenerator,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _entityIdGenerator = entityIdGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<CommandResult> Handle(SignUpCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await User.Create(request.Email, request.Password,
            _userEmailUniquenessChecker, _entityIdGenerator, _passwordHasher);

        await customer.AddCustomerRole(_entityIdGenerator);

        _repository.Add(customer);

        await _unitOfWork.Commit();
        return CommandResult.Completed;
    }
}
