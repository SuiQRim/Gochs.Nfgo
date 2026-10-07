using System.ComponentModel.DataAnnotations;
using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Notifications;

public class UpdateNotificationStatusDto
{
    [Required]
    public NotificationStatus? Status { get; set; }
}
