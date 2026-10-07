using Gochs.Nfgo.DTOs.Formations;

namespace Gochs.Nfgo.Services.Interfaces;

public interface IFormationService
{
    Task<IReadOnlyCollection<FormationDto>> GetAllAsync();
    Task<FormationDetailsDto> GetByIdAsync(int id);
    Task<FormationDto> CreateAsync(CreateFormationDto dto);
    Task<FormationDto> UpdateAsync(int id, UpdateFormationDto dto);
    Task DeleteAsync(int id);
}
