using Gochs.Nfgo.Data;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Repositories.Implementations;

public class FormationRepository : IFormationRepository
{
    private readonly AppDbContext context;

    public FormationRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<List<Formation>> GetAllAsync() =>
        context.Formations.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    public Task<Formation?> GetByIdAsync(int id) =>
        context.Formations.FirstOrDefaultAsync(x => x.Id == id);

    public Task<Formation?> GetDetailsAsync(int id) =>
        context.Formations
            .AsNoTracking()
            .Include(x => x.Units)
                .ThenInclude(x => x.Employees)
            .Include(x => x.Units)
                .ThenInclude(x => x.Equipment)
            .FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> ExistsAsync(int id) =>
        context.Formations.AnyAsync(x => x.Id == id);

    public Task<bool> HasUnitsAsync(int id) =>
        context.Units.AnyAsync(x => x.FormationId == id);

    public Task<bool> HasNotificationsAsync(int id) =>
        context.Notifications.AnyAsync(x => x.FormationId == id);

    public async Task AddAsync(Formation entity)
    {
        context.Formations.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Formation entity)
    {
        context.Formations.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Formation entity)
    {
        context.Formations.Remove(entity);
        await context.SaveChangesAsync();
    }
}
