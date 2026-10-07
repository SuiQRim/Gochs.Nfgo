using Gochs.Nfgo.DTOs.Formations;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/formations")]
public class FormationsController : ControllerBase
{
    private readonly IFormationService formationService;

    public FormationsController(IFormationService formationService)
    {
        this.formationService = formationService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FormationDto>>> GetAll()
    {
        return Ok(await formationService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FormationDetailsDto>> GetById(int id)
    {
        return Ok(await formationService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<FormationDto>> Create(CreateFormationDto dto)
    {
        var result = await formationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FormationDto>> Update(int id, UpdateFormationDto dto)
    {
        return Ok(await formationService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await formationService.DeleteAsync(id);
        return NoContent();
    }
}
