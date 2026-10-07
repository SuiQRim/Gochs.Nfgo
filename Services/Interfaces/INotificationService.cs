using Gochs.Nfgo.DTOs.Notifications;

namespace Gochs.Nfgo.Services.Interfaces;

public interface INotificationService
{
    Task<IReadOnlyCollection<NotificationDto>> GetAllAsync();
    Task<IReadOnlyCollection<NotificationDto>> GetByFormationIdAsync(int formationId);
    Task<NotificationDto> GetByIdAsync(int id);
    Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
    Task<NotificationDto> UpdateStatusAsync(int id, UpdateNotificationStatusDto dto);
}
