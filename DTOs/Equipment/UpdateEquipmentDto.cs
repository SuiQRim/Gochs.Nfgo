using System.ComponentModel.DataAnnotations;
using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Equipment;

public class UpdateEquipmentDto
{
    [Range(1, int.MaxValue)]
    public int UnitId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    public EquipmentType? Type { get; set; }

    [StringLength(100)]
    public string? InventoryNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public EquipmentCondition? Condition { get; set; }
}
