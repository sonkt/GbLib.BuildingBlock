using GbLib.Domain.Interfaces;

namespace GbLib.Domain.Specifications;

public class AndSpecification<T> : BaseSpecification<T>
{
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Criteria = x => left.Criteria.Compile()(x) && right.Criteria.Compile()(x);
    }
}
