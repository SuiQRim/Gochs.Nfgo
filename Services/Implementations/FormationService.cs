using Gochs.Nfgo.DTOs.Formations;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Exceptions;
using Gochs.Nfgo.Mappings;
using Gochs.Nfgo.Repositories.Interfaces;
using Gochs.Nfgo.Services.Interfaces;

namespace Gochs.Nfgo.Services.Implementations;

public class FormationService : IFormationService
{
    private readonly IFormationRepository formationRepository;

    public FormationService(IFormationRepository formationRepository)
    {
        this.formationRepository = formationRepository;
    }

    public async Task<IReadOnlyCollection<FormationDto>> GetAllAsync() =>
        (await formationRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<FormationDetailsDto> GetByIdAsync(int id)
    {
        var formation = await formationRepository.GetDetailsAsync(id)
            ?? throw new NotFoundException($"Формирование с id {id} не найдено.");

        return new FormationDetailsDto
        {
            Id = formation.Id,
            Name = formation.Name,
            Type = formation.Type,
            Purpose = formation.Purpose,
            Location = formation.Location,
            LeaderName = formation.LeaderName,
            Status = formation.Status,
            CreatedAt = formation.CreatedAt,
            EmployeeCount = formation.Units.Sum(x => x.Employees.Count),
            EquipmentCount = formation.Units.Sum(x => x.Equipment.Sum(y => y.Quantity)),
            Units = formation.Units.OrderBy(x => x.Name).Select(x => x.ToDto()).ToList()
        };
    }

    public async Task<FormationDto> CreateAsync(CreateFormationDto dto)
    {
        var name = RequireText(dto.Name, "Название формирования");
        var location = RequireText(dto.Location, "Место расположения формирования");

        var entity = new Formation
        {
            Name = name,
            Type = dto.Type!.Value,
            Purpose = Normalize(dto.Purpose),
            Location = location,
            LeaderName = Normalize(dto.LeaderName)
        };

        await formationRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<FormationDto> UpdateAsync(int id, UpdateFormationDto dto)
    {
        var entity = await formationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Формирование с id {id} не найдено.");

        entity.Name = RequireText(dto.Name, "Название формирования");
        entity.Type = dto.Type!.Value;
        entity.Purpose = Normalize(dto.Purpose);
        entity.Location = RequireText(dto.Location, "Место расположения формирования");
        entity.LeaderName = Normalize(dto.LeaderName);
        entity.Status = dto.Status!.Value;

        await formationRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await formationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Формирование с id {id} не найдено.");

        if (await formationRepository.HasUnitsAsync(id))
            throw new BusinessRuleException("Нельзя удалить формирование, пока в нём есть подразделения.");

        if (await formationRepository.HasNotificationsAsync(id))
            throw new BusinessRuleException("Нельзя удалить формирование, пока у него есть история оповещений.");

        await formationRepository.DeleteAsync(entity);
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
