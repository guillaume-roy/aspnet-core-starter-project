using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

using ServerDomain.Kernel.Entities;

namespace ServerCore.Kernel.Repositories;

public interface IWriteRepository<T> where T : EntityBase, IAggregateRoot
{
    void Add(T entity);

    Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null);
}