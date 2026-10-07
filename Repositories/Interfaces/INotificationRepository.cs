using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync();
    Task<List<Notification>> GetByFormationIdAsync(int formationId);
    Task<Notification?> GetByIdAsync(int id);
    Task AddAsync(Notification entity);
    Task UpdateAsync(Notification entity);
}
