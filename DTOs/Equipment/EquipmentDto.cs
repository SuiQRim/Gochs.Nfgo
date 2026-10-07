using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Equipment;

public class EquipmentDto
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string Name { get; set; } = null!;
    public EquipmentType Type { get; set; }
    public string? InventoryNumber { get; set; }
    public int Quantity { get; set; }
    public EquipmentCondition Condition { get; set; }
    public DateTime CreatedAt { get; set; }
}
