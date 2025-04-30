using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GbLib.BuildingBlock.Infrastructure.Persistence;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        query = query.Where(spec.Criteria);

        if (spec.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec is { IsPagingEnabled: true, Skip: not null, Take: not null })
            query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
    }
}