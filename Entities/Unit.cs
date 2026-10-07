namespace Gochs.Nfgo.Entities;

public class Unit
{
    public int Id { get; set; }
    public int FormationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Purpose { get; set; }
    public string? LeaderName { get; set; }
    public DateTime CreatedAt { get; set; }

    public Formation Formation { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
