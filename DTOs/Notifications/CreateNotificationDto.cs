using System.ComponentModel.DataAnnotations;

namespace Gochs.Nfgo.DTOs.Notifications;

public class CreateNotificationDto
{
    [Range(1, int.MaxValue)]
    public int FormationId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string CreatedBy { get; set; } = null!;
}
