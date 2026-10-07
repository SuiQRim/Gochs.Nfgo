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
            ?? throw new NotFoundException($"Formation with id {id} was not found.");

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
        var name = RequireText(dto.Name, "Formation name");
        var location = RequireText(dto.Location, "Formation location");

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
            ?? throw new NotFoundException($"Formation with id {id} was not found.");

        entity.Name = RequireText(dto.Name, "Formation name");
        entity.Type = dto.Type!.Value;
        entity.Purpose = Normalize(dto.Purpose);
        entity.Location = RequireText(dto.Location, "Formation location");
        entity.LeaderName = Normalize(dto.LeaderName);
        entity.Status = dto.Status!.Value;

        await formationRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await formationRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Formation with id {id} was not found.");

        if (await formationRepository.HasUnitsAsync(id))
            throw new BusinessRuleException("Formation cannot be deleted while it contains units.");

        if (await formationRepository.HasNotificationsAsync(id))
            throw new BusinessRuleException("Formation cannot be deleted while it has notification history.");

        await formationRepository.DeleteAsync(entity);
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} cannot be empty.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
