namespace GbLib.UnitTest.Models;

public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ChildEntity> Items { get; set; }
}

public class ChildEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}