using MediatR;

namespace ServerCore.Kernel.Queries;

public interface IQuery<TResult> : IRequest<TResult>
{
}
