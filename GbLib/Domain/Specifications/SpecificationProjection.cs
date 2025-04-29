using AutoMapper;
using AutoMapper.QueryableExtensions;
using GbLib.Domain.Interfaces;
using GbLib.Infrastructure.Persistence;

namespace GbLib.Domain.Specifications;

public static class SpecificationProjection
{
    public static IQueryable<TResult> ProjectToResult<T, TResult>(
        this IQueryable<T> source,
        ISpecification<T> specification,
        IConfigurationProvider configuration) where T : class
    {
        var query = SpecificationEvaluator<T>.GetQuery(source, specification);
        return query.ProjectTo<TResult>(configuration);
    }
}