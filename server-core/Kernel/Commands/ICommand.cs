using MediatR;

namespace ServerCore.Kernel.Commands;

public interface ICommand : IRequest<CommandResult>
{
}
