using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;

using ServerCore.Kernel.Repositories;

using ServerDomain.Kernel.Entities;

using ServerInfrastructure.Database.Extensions;

namespace ServerTests.Fakes;

public class FakeWriteRepository<T> : IWriteRepository<T> where T : EntityBase, IAggregateRoot
{
    public List<T> Entities { get; set; } = new List<T>();

    public void Add(T entity)
    {
        Entities.Add(entity);
    }

    public Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null)
    {
        return Task.FromResult(Entities.AsQueryable().ApplyPredicate(predicate).SingleOrDefault());
    }
}