using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.Entities;

public class Equipment
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string Name { get; set; } = null!;
    public EquipmentType Type { get; set; }
    public string? InventoryNumber { get; set; }
    public int Quantity { get; set; } = 1;
    public EquipmentCondition Condition { get; set; } = EquipmentCondition.Good;
    public DateTime CreatedAt { get; set; }

    public Unit Unit { get; set; } = null!;
}
