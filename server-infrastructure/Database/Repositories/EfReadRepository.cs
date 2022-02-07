using System.Linq.Expressions;
using ServerInfrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;

using ServerCore.Kernel.Repositories;

using ServerDomain.Kernel.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace ServerInfrastructure.Database.Repositories;

public class EfReadRepository<T> : IReadRepository<T> where T : EntityBase
{
    private readonly ApplicationDbContext _dbContext;

    public EfReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> Exists(Expression<Func<T, bool>> predicate = null)
    {
        return GetBaseQuery()
            .ApplyPredicate(predicate)
            .AnyAsync();
    }

    public Task<bool> NotExists(Expression<Func<T, bool>> predicate = null)
    {
        return Exists(predicate).ContinueWith(t => !t.Result);
    }

    public Task<List<T>> GetAll(Expression<Func<T, bool>> predicate = null)
    {
        return GetBaseQuery()
            .ApplyPredicate(predicate)
            .ToListAsync();
    }

    public Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null)
    {
        return GetBaseQuery()
            .ApplyPredicate(predicate)
            .ApplyIncludes(includes)
            .SingleOrDefaultAsync();
    }

    private IQueryable<T> GetBaseQuery()
    {
        return _dbContext.Set<T>().AsNoTracking();
    }
}