using Gochs.Nfgo.Data;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Repositories.Implementations;

public class UnitRepository : IUnitRepository
{
    private readonly AppDbContext context;

    public UnitRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<List<Unit>> GetAllAsync() =>
        context.Units.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public Task<List<Unit>> GetByFormationIdAsync(int formationId) =>
        context.Units.AsNoTracking()
            .Where(x => x.FormationId == formationId)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public Task<Unit?> GetByIdAsync(int id) =>
        context.Units.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> ExistsAsync(int id) =>
        context.Units.AnyAsync(x => x.Id == id);

    public Task<bool> HasEmployeesAsync(int id) =>
        context.Employees.AnyAsync(x => x.UnitId == id);

    public Task<bool> HasEquipmentAsync(int id) =>
        context.Equipment.AnyAsync(x => x.UnitId == id);

    public async Task AddAsync(Unit entity)
    {
        context.Units.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Unit entity)
    {
        context.Units.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Unit entity)
    {
        context.Units.Remove(entity);
        await context.SaveChangesAsync();
    }
}
