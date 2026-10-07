using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public int FormationId { get; set; }
    public string Message { get; set; } = null!;
    public NotificationStatus Status { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
