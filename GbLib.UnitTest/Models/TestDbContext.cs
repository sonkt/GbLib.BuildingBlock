using Microsoft.EntityFrameworkCore;

namespace GbLib.UnitTest.Models;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    public DbSet<TestEntity> Entities { get; set; }
}
