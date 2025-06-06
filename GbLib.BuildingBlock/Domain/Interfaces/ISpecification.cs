using System.Linq.Expressions;

namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<string> IncludeStrings { get; }

    List<Expression<Func<T, object>>> Includes { get; }

    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

    int? Take { get; }
    int? Skip { get; }
    bool IsPagingEnabled { get; }
    bool AsNoTracking { get; }
}