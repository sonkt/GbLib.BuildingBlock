using GbLib.BuildingBlock.Domain.Specifications;

namespace GbLib.UnitTest.Models;

public class ApplyOrderSpecification:BaseSpecification<TestEntity>
{
    public ApplyOrderSpecification()
    {
        ApplyOrderByDescending(x=>x.Name);
    }
}