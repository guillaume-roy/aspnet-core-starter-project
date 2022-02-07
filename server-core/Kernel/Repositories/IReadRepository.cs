using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

using ServerDomain.Kernel.Entities;

namespace ServerCore.Kernel.Repositories;

public interface IReadRepository<T> where T : EntityBase
{
    Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);

    Task<bool> Exists(Expression<Func<T, bool>> predicate);

    Task<bool> NotExists(Expression<Func<T, bool>> predicate);

    Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null);
}