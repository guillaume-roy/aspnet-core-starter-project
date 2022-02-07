using MediatR;

namespace ServerCore.Kernel.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CommandResult> where TCommand : ICommand
{
    public new Task<CommandResult> Handle(TCommand request, CancellationToken cancellationToken);
}
