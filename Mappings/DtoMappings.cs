using Gochs.Nfgo.DTOs.Employees;
using Gochs.Nfgo.DTOs.Equipment;
using Gochs.Nfgo.DTOs.Formations;
using Gochs.Nfgo.DTOs.Notifications;
using Gochs.Nfgo.DTOs.Units;
using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Mappings;

public static class DtoMappings
{
    public static FormationDto ToDto(this Formation entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Type = entity.Type,
        Purpose = entity.Purpose,
        Location = entity.Location,
        LeaderName = entity.LeaderName,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt
    };

    public static UnitDto ToDto(this Unit entity) => new()
    {
        Id = entity.Id,
        FormationId = entity.FormationId,
        Name = entity.Name,
        Purpose = entity.Purpose,
        LeaderName = entity.LeaderName,
        CreatedAt = entity.CreatedAt
    };

    public static EmployeeDto ToDto(this Employee entity) => new()
    {
        Id = entity.Id,
        UnitId = entity.UnitId,
        PersonnelNumber = entity.PersonnelNumber,
        FullName = entity.FullName,
        Position = entity.Position,
        Phone = entity.Phone,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt
    };

    public static EquipmentDto ToDto(this Equipment entity) => new()
    {
        Id = entity.Id,
        UnitId = entity.UnitId,
        Name = entity.Name,
        Type = entity.Type,
        InventoryNumber = entity.InventoryNumber,
        Quantity = entity.Quantity,
        Condition = entity.Condition,
        CreatedAt = entity.CreatedAt
    };

    public static NotificationDto ToDto(this Notification entity) => new()
    {
        Id = entity.Id,
        FormationId = entity.FormationId,
        Message = entity.Message,
        Status = entity.Status,
        CreatedBy = entity.CreatedBy,
        CreatedAt = entity.CreatedAt
    };
}
