using GbLib.BuildingBlock.Domain.Specifications;

namespace GbLib.UnitTest.Models;

public class PagingCriterialSpecification: BaseSpecification<TestEntity>
{
    public PagingCriterialSpecification(int page, int size)
    {
        Criteria = m => true;
        ApplyPaging(page, size);
    }
}