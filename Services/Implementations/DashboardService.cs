using Gochs.Nfgo.Data;
using Gochs.Nfgo.DTOs.Dashboard;
using Gochs.Nfgo.Enums;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext context;

    public DashboardService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<DashboardDto> GetAsync()
    {
        return new DashboardDto
        {
            FormationCount = await context.Formations.CountAsync(),
            UnitCount = await context.Units.CountAsync(),
            EmployeeCount = await context.Employees.CountAsync(),
            EquipmentCount = await context.Equipment.SumAsync(x => x.Quantity),
            ReadyFormationCount = await context.Formations.CountAsync(x => x.Status == FormationStatus.Ready),
            RequiresAttentionFormationCount = await context.Formations.CountAsync(x => x.Status == FormationStatus.RequiresAttention),
            InactiveFormationCount = await context.Formations.CountAsync(x => x.Status == FormationStatus.Inactive),
            EquipmentRequiringRepairCount = await context.Equipment
                .Where(x => x.Condition == EquipmentCondition.RequiresRepair)
                .SumAsync(x => x.Quantity),
            UnusableEquipmentCount = await context.Equipment
                .Where(x => x.Condition == EquipmentCondition.Unusable)
                .SumAsync(x => x.Quantity)
        };
    }
}
