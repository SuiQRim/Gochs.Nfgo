namespace Gochs.Nfgo.DTOs.Units;

public class UnitDto
{
    public int Id { get; set; }
    public int FormationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Purpose { get; set; }
    public string? LeaderName { get; set; }
    public DateTime CreatedAt { get; set; }
}
