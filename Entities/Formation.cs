using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.Entities;

public class Formation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public FormationType Type { get; set; }
    public string? Purpose { get; set; }
    public string Location { get; set; } = null!;
    public string? LeaderName { get; set; }
    public FormationStatus Status { get; set; } = FormationStatus.Ready;
    public DateTime CreatedAt { get; set; }

    public ICollection<Unit> Units { get; set; } = new List<Unit>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
