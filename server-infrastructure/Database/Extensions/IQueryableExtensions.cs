using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using ServerDomain.Kernel.Entities;

namespace ServerInfrastructure.Database.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPredicate<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate) where T : EntityBase
    {
        return predicate == null ? query : query.Where(predicate);
    }

    public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, Func<IQueryable<T>, IIncludableQueryable<T, object>>[] includes) where T : EntityBase
    {
        if (includes == null)
        {
            return query;
        }

        foreach (Func<IQueryable<T>, IIncludableQueryable<T, object>> include in includes)
        {
            query = include(query);
        }

        return query;
    }
}