using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.Entities;

public class Notification
{
    public int Id { get; set; }
    public int FormationId { get; set; }
    public string Message { get; set; } = null!;
    public NotificationStatus Status { get; set; } = NotificationStatus.Created;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Formation Formation { get; set; } = null!;
}
