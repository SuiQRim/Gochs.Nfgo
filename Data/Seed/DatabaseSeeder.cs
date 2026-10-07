using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Formations.AnyAsync())
            return;

        var rescue = new Formation
        {
            Name = "Аварийно-спасательное формирование №1",
            Type = FormationType.Rescue,
            Purpose = "Проведение аварийно-спасательных и неотложных работ",
            Location = "Корпус №1",
            LeaderName = "Руководитель формирования 1",
            Status = FormationStatus.Ready
        };

        var engineering = new Formation
        {
            Name = "Инженерное формирование",
            Type = FormationType.Engineering,
            Purpose = "Инженерное обеспечение мероприятий гражданской обороны",
            Location = "Техническая база",
            LeaderName = "Руководитель формирования 2",
            Status = FormationStatus.RequiresAttention
        };

        var medical = new Formation
        {
            Name = "Медицинское формирование",
            Type = FormationType.Medical,
            Purpose = "Оказание первой помощи и медицинское сопровождение",
            Location = "Административный корпус",
            LeaderName = "Руководитель формирования 3",
            Status = FormationStatus.Inactive
        };

        var rescueUnit = new Unit
        {
            Name = "Спасательная группа",
            Purpose = "Поисково-спасательные работы",
            LeaderName = "Руководитель группы 1",
            Formation = rescue
        };

        var engineeringUnit = new Unit
        {
            Name = "Инженерная группа",
            Purpose = "Разбор завалов и инженерные работы",
            LeaderName = "Руководитель группы 2",
            Formation = engineering
        };

        var powerUnit = new Unit
        {
            Name = "Группа энергоснабжения",
            Purpose = "Резервное электроснабжение объектов",
            LeaderName = "Руководитель группы 3",
            Formation = engineering
        };

        var medicalUnit = new Unit
        {
            Name = "Медицинская группа",
            Purpose = "Оказание первой помощи",
            LeaderName = "Руководитель группы 4",
            Formation = medical
        };

        context.Employees.AddRange(
            new Employee { PersonnelNumber = "NFGO-001", FullName = "Сотрудник 001", Position = "Спасатель", Unit = rescueUnit },
            new Employee { PersonnelNumber = "NFGO-002", FullName = "Сотрудник 002", Position = "Спасатель", Unit = rescueUnit },
            new Employee { PersonnelNumber = "NFGO-003", FullName = "Сотрудник 003", Position = "Инженер", Unit = engineeringUnit },
            new Employee { PersonnelNumber = "NFGO-004", FullName = "Сотрудник 004", Position = "Механик", Unit = engineeringUnit, Status = EmployeeStatus.Unavailable },
            new Employee { PersonnelNumber = "NFGO-005", FullName = "Сотрудник 005", Position = "Электромонтёр", Unit = powerUnit },
            new Employee { PersonnelNumber = "NFGO-006", FullName = "Сотрудник 006", Position = "Врач", Unit = medicalUnit },
            new Employee { PersonnelNumber = "NFGO-007", FullName = "Сотрудник 007", Position = "Фельдшер", Unit = medicalUnit });

        context.Equipment.AddRange(
            new Equipment { Name = "Комплект спасательного инструмента", Type = EquipmentType.Rescue, InventoryNumber = "NFGO-EQ-001", Quantity = 3, Condition = EquipmentCondition.Good, Unit = rescueUnit },
            new Equipment { Name = "Гидравлический инструмент", Type = EquipmentType.Rescue, InventoryNumber = "NFGO-EQ-002", Quantity = 2, Condition = EquipmentCondition.RequiresRepair, Unit = engineeringUnit },
            new Equipment { Name = "Дизельный генератор", Type = EquipmentType.Other, InventoryNumber = "NFGO-EQ-003", Quantity = 2, Condition = EquipmentCondition.RequiresRepair, Unit = powerUnit },
            new Equipment { Name = "Медицинская укладка", Type = EquipmentType.Medical, InventoryNumber = "NFGO-EQ-004", Quantity = 3, Condition = EquipmentCondition.Good, Unit = medicalUnit },
            new Equipment { Name = "Радиостанция переносная", Type = EquipmentType.Communication, InventoryNumber = "NFGO-EQ-005", Quantity = 6, Condition = EquipmentCondition.Unusable, Unit = rescueUnit });

        context.Notifications.AddRange(
            new Notification { Formation = rescue, Message = "Проверка готовности формирования.", CreatedBy = "Дежурный ГО", Status = NotificationStatus.Sent },
            new Notification { Formation = engineering, Message = "Провести проверку технического состояния оборудования.", CreatedBy = "Начальник штаба ГО", Status = NotificationStatus.Created },
            new Notification { Formation = medical, Message = "Подготовить имущество к учебной тренировке.", CreatedBy = "Начальник штаба ГО", Status = NotificationStatus.Failed });

        await context.SaveChangesAsync();
    }
}
