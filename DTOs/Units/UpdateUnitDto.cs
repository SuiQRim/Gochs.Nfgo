using System.ComponentModel.DataAnnotations;

namespace Gochs.Nfgo.DTOs.Units;

public class UpdateUnitDto
{
    [Range(1, int.MaxValue)]
    public int FormationId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Purpose { get; set; }

    [StringLength(200)]
    public string? LeaderName { get; set; }
}
