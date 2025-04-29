using GbLib.Domain.Interfaces;

namespace GbLib.Domain.Specifications;

public class OrSpecification<T> : BaseSpecification<T>
{
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Criteria = x => left.Criteria.Compile()(x) || right.Criteria.Compile()(x);
    }
}