using GbLib.BuildingBlock.Domain.Specifications;

namespace GbLib.UnitTest.Models;

public class CriteriaSpecification: BaseSpecification<TestEntity>
{
    public CriteriaSpecification(string name)
    {
        Criteria = m => m.Name == name;
    }
}