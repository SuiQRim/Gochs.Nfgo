using System.ComponentModel.DataAnnotations;
using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Formations;

public class UpdateFormationDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    public FormationType? Type { get; set; }

    [StringLength(500)]
    public string? Purpose { get; set; }

    [Required]
    [StringLength(300)]
    public string Location { get; set; } = null!;

    [StringLength(200)]
    public string? LeaderName { get; set; }

    [Required]
    public FormationStatus? Status { get; set; }
}
