using GbLib.BuildingBlock.Infrastructure.Persistence;
using GbLib.UnitTest.Models;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace GbLib.UnitTest;

public class SpecificationEvaluatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SpecificationEvaluatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetQuery_FiltersByCriteria()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity{ Id = 1, Name = "Alice" },
            new TestEntity{ Id = 2, Name = "Bob" }
        }.AsQueryable();

        var spec = new CriteriaSpecification("Bob");

        // Act
        var result = SpecificationEvaluator<TestEntity>.GetQuery(data, spec).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Bob", result[0].Name);
    }

    [Fact]
    public void GetQuery_AppliesPaging()
    {
        // Arrange
        var data = Enumerable.Range(1, 10)
            .Select(i => new TestEntity { Id = i, Name = $"Name{i}" }).AsQueryable();

        var spec = new PagingCriterialSpecification(5,2);
        // Act
        var result = SpecificationEvaluator<TestEntity>.GetQuery(data, spec).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(6, result[0].Id);
        Assert.Equal(7, result[1].Id);
    }

    [Fact]
    public void GetQuery_AppliesOrderByDescending()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new TestEntity{ Id = 1, Name = "Alice" },
            new TestEntity{ Id = 2, Name = "Bob" }
        }.AsQueryable();

        var spec = new ApplyOrderSpecification();

        var query = SpecificationEvaluator<TestEntity>.GetQuery(data, spec);
        var sql = query.ToQueryString();
        _testOutputHelper.WriteLine(sql);
        // Assert
        var result = query.ToList();
        _testOutputHelper.WriteLine(result.Count.ToString());
        Assert.Equal(2, result.Count);
        Assert.Equal("Bob", result[0].Name);
        Assert.Equal("Alice", result[1].Name);
    }

    [Fact]
    public async Task GetQuery_ReturnsFilteredOrderedPagedItems()
    {
        // Arrange: DbContext in-memory
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        await using var context = new TestDbContext(options);
        await context.Database.OpenConnectionAsync(); // Đảm bảo có kết nối, với SQLite in-memory
        await context.Database.EnsureCreatedAsync();  // Tạo toàn bộ bảng theo DbContext
        context.Entities.AddRange(
            new TestEntity { Id = 1, Name = "A", Items =
                [
                    new ChildEntity { Id = 1, Name = "A1" },
                    new ChildEntity { Id = 2, Name = "A2" }
                ]
            },
            new TestEntity { Id = 2, Name = "B", Items =
            [
                new ChildEntity { Id = 3, Name = "B1" },
                new ChildEntity { Id = 4, Name = "B2" }
            ] },
            new TestEntity { Id = 3, Name = "C", Items =
            [
                new ChildEntity { Id = 5, Name = "C1" },
                new ChildEntity { Id = 6, Name = "C2" }
            ] }
        );
        await context.SaveChangesAsync();

        var spec = new TestSpecification(1, 10);

        var query = SpecificationEvaluator<TestEntity>.GetQuery(context.Entities.AsQueryable(), spec);
        var results = await query.ToListAsync();
        _testOutputHelper.WriteLine(query.ToQueryString());

        Assert.Equal("C", results[0].Name);
        Assert.Equal("B", results[1].Name);
    }

}