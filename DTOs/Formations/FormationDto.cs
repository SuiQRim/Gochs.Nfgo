using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Formations;

public class FormationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public FormationType Type { get; set; }
    public string? Purpose { get; set; }
    public string Location { get; set; } = null!;
    public string? LeaderName { get; set; }
    public FormationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
