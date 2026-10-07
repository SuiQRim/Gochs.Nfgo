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

        var formation = new Formation
        {
            Name = "Аварийно-спасательное формирование №1",
            Type = FormationType.Rescue,
            Purpose = "Учебное формирование НФГО",
            Location = "Корпус №1",
            LeaderName = "Руководитель формирования",
            Status = FormationStatus.Ready
        };

        var unit = new Unit
        {
            Name = "Спасательная группа",
            Purpose = "Поисково-спасательные работы",
            LeaderName = "Руководитель группы",
            Formation = formation
        };

        context.Employees.AddRange(
            new Employee { PersonnelNumber = "NFGO-001", FullName = "Сотрудник 001", Position = "Спасатель", Unit = unit },
            new Employee { PersonnelNumber = "NFGO-002", FullName = "Сотрудник 002", Position = "Спасатель", Unit = unit });

        context.Equipment.Add(
            new Equipment
            {
                Name = "Комплект спасательного инструмента",
                Type = EquipmentType.Rescue,
                InventoryNumber = "NFGO-EQ-001",
                Quantity = 2,
                Condition = EquipmentCondition.Good,
                Unit = unit
            });

        context.Notifications.Add(
            new Notification
            {
                Formation = formation,
                Message = "Проверка готовности формирования.",
                CreatedBy = "Дежурный ГО",
                Status = NotificationStatus.Sent
            });

        await context.SaveChangesAsync();
    }
}
