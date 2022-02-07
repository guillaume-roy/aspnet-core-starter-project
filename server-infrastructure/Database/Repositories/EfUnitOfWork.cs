using ServerCore.Kernel.Repositories;

namespace ServerInfrastructure.Database.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public EfUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}