using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GbLib.BuildingBlock.Infrastructure.Persistence;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        query = query.Where(spec.Criteria);

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled && spec.Skip.HasValue && spec.Take.HasValue)
            query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        foreach (var include in spec.IncludeStrings)
        {
            query = query.Include(include);
        }

        return query;
    }
}