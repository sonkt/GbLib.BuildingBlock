using System.Linq.Expressions;
using GbLib.BuildingBlock.Domain.Interfaces;
using GbLib.BuildingBlock.Domain.Specifications;

namespace GbLib.UnitTest.Models;

public class TestSpecification : BaseSpecification<TestEntity>
{
    public TestSpecification(int page = 1, int size = 10)
    {
        var skip = (page - 1) * size;
        Criteria = m => m.Id>0;
        AddInclude(x => x.Items);
        ApplyPaging(skip, size);
        ApplyOrderByDescending(o => o.Id);
        ApplyAsNoTracking();
    }
}
