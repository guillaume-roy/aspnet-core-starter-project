using System.Threading.Tasks;

using ServerCore.Kernel.Repositories;

namespace ServerTests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task Commit()
    {
        return Task.CompletedTask;
    }
}