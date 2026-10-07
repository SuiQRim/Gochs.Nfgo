using Gochs.Nfgo.Data;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Repositories.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext context;

    public NotificationRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<List<Notification>> GetAllAsync() =>
        context.Notifications.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public Task<List<Notification>> GetByFormationIdAsync(int formationId) =>
        context.Notifications.AsNoTracking()
            .Where(x => x.FormationId == formationId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public Task<Notification?> GetByIdAsync(int id) =>
        context.Notifications.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Notification entity)
    {
        context.Notifications.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notification entity)
    {
        context.Notifications.Update(entity);
        await context.SaveChangesAsync();
    }
}
