using Gochs.Nfgo.DTOs.Units;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/units")]
public class UnitsController : ControllerBase
{
    private readonly IUnitService unitService;

    public UnitsController(IUnitService unitService)
    {
        this.unitService = unitService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UnitDto>>> GetAll()
    {
        return Ok(await unitService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnitDto>> GetById(int id)
    {
        return Ok(await unitService.GetByIdAsync(id));
    }

    [HttpGet("/api/formations/{formationId:int}/units")]
    public async Task<ActionResult<IReadOnlyCollection<UnitDto>>> GetByFormation(int formationId)
    {
        return Ok(await unitService.GetByFormationIdAsync(formationId));
    }

    [HttpPost]
    public async Task<ActionResult<UnitDto>> Create(CreateUnitDto dto)
    {
        var result = await unitService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UnitDto>> Update(int id, UpdateUnitDto dto)
    {
        return Ok(await unitService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await unitService.DeleteAsync(id);
        return NoContent();
    }
}
