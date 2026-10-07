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
            ?? throw new NotFoundException($"Оповещение с id {id} не найдено.");

        return entity.ToDto();
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        await EnsureFormationExistsAsync(dto.FormationId);

        var entity = new Notification
        {
            FormationId = dto.FormationId,
            Message = RequireText(dto.Message, "Текст оповещения"),
            CreatedBy = RequireText(dto.CreatedBy, "Автор оповещения")
        };

        await notificationRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<NotificationDto> UpdateStatusAsync(int id, UpdateNotificationStatusDto dto)
    {
        var entity = await notificationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Оповещение с id {id} не найдено.");

        var newStatus = dto.Status!.Value;

        if (entity.Status != NotificationStatus.Created)
            throw new BusinessRuleException("Изменять можно только оповещение в статусе Created.");

        if (newStatus is not NotificationStatus.Sent and not NotificationStatus.Failed)
            throw new BusinessRuleException("Оповещение может перейти из Created только в Sent или Failed.");

        entity.Status = newStatus;
        await notificationRepository.UpdateAsync(entity);

        return entity.ToDto();
    }

    private async Task EnsureFormationExistsAsync(int id)
    {
        if (!await formationRepository.ExistsAsync(id))
            throw new NotFoundException($"Формирование с id {id} не найдено.");
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }
}
