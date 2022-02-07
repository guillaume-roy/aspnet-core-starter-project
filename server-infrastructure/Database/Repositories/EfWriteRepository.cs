using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using ServerCore.Kernel.Repositories;

using ServerDomain.Kernel.Entities;

using ServerInfrastructure.Database.Extensions;

namespace ServerInfrastructure.Database.Repositories;

public class EfWriteRepository<T> : IWriteRepository<T> where T : EntityBase, IAggregateRoot
{
    private readonly ApplicationDbContext _dbContext;

    public EfWriteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null)
    {
        return _dbContext.Set<T>()
            .ApplyPredicate(predicate)
            .ApplyIncludes(includes)
            .SingleOrDefaultAsync();
    }
}