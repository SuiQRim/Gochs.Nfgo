using Gochs.Nfgo.DTOs.Notifications;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Enums;
using Gochs.Nfgo.Exceptions;
using Gochs.Nfgo.Mappings;
using Gochs.Nfgo.Repositories.Interfaces;
using Gochs.Nfgo.Services.Interfaces;

namespace Gochs.Nfgo.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository notificationRepository;
    private readonly IFormationRepository formationRepository;

    public NotificationService(
        INotificationRepository notificationRepository,
        IFormationRepository formationRepository)
    {
        this.notificationRepository = notificationRepository;
        this.formationRepository = formationRepository;
    }

    public async Task<IReadOnlyCollection<NotificationDto>> GetAllAsync() =>
        (await notificationRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<NotificationDto>> GetByFormationIdAsync(int formationId)
    {
        await EnsureFormationExistsAsync(formationId);
        return (await notificationRepository.GetByFormationIdAsync(formationId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<NotificationDto> GetByIdAsync(int id)
    {
        var entity = await notificationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Notification with id {id} was not found.");

        return entity.ToDto();
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        await EnsureFormationExistsAsync(dto.FormationId);

        var entity = new Notification
        {
            FormationId = dto.FormationId,
            Message = dto.Message.Trim(),
            CreatedBy = dto.CreatedBy.Trim()
        };

        await notificationRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<NotificationDto> UpdateStatusAsync(int id, UpdateNotificationStatusDto dto)
    {
        var entity = await notificationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Notification with id {id} was not found.");

        var newStatus = dto.Status!.Value;

        if (entity.Status != NotificationStatus.Created)
            throw new BusinessRuleException("Only notifications in Created status can be updated.");

        if (newStatus is not NotificationStatus.Sent and not NotificationStatus.Failed)
            throw new BusinessRuleException("Notification can only transition from Created to Sent or Failed.");

        entity.Status = newStatus;
        await notificationRepository.UpdateAsync(entity);

        return entity.ToDto();
    }

    private async Task EnsureFormationExistsAsync(int id)
    {
        if (!await formationRepository.ExistsAsync(id))
            throw new NotFoundException($"Formation with id {id} was not found.");
    }
}
