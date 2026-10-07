using Gochs.Nfgo.Data;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Repositories.Implementations;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext context;

    public EquipmentRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<List<Equipment>> GetAllAsync() =>
        context.Equipment.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public Task<List<Equipment>> GetByUnitIdAsync(int unitId) =>
        context.Equipment.AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public Task<List<Equipment>> GetByFormationIdAsync(int formationId) =>
        context.Equipment.AsNoTracking()
            .Where(x => x.Unit.FormationId == formationId)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public Task<Equipment?> GetByIdAsync(int id) =>
        context.Equipment.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> InventoryNumberExistsAsync(string inventoryNumber, int? excludeId = null) =>
        context.Equipment.AnyAsync(x =>
            x.InventoryNumber == inventoryNumber &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

    public async Task AddAsync(Equipment entity)
    {
        context.Equipment.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Equipment entity)
    {
        context.Equipment.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Equipment entity)
    {
        context.Equipment.Remove(entity);
        await context.SaveChangesAsync();
    }
}
