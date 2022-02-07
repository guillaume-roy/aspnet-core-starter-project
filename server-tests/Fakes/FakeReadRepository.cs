using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;

using ServerCore.Kernel.Repositories;

using ServerDomain.Kernel.Entities;

namespace ServerTests.Fakes;

public class FakeReadRepository<T> : IReadRepository<T> where T : EntityBase
{
    public List<T> Entities { get; set; } = new List<T>();

    public Task<bool> Exists(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(Entities.AsQueryable().Where(predicate).Any());
    }

    public Task<T?> Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes = null)
    {
        return Task.FromResult(Entities.AsQueryable().Where(predicate).SingleOrDefault());
    }

    public Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(Entities.AsQueryable().Where(predicate).ToList());
    }

    public Task<bool> NotExists(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult(!Entities.AsQueryable().Where(predicate).Any());
    }
}