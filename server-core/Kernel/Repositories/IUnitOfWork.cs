namespace ServerCore.Kernel.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}