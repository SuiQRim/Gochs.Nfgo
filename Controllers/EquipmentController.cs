using Gochs.Nfgo.DTOs.Equipment;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/equipment")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        this.equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetAll()
    {
        return Ok(await equipmentService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EquipmentDto>> GetById(int id)
    {
        return Ok(await equipmentService.GetByIdAsync(id));
    }

    [HttpGet("/api/units/{unitId:int}/equipment")]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetByUnit(int unitId)
    {
        return Ok(await equipmentService.GetByUnitIdAsync(unitId));
    }

    [HttpGet("/api/formations/{formationId:int}/equipment")]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetByFormation(int formationId)
    {
        return Ok(await equipmentService.GetByFormationIdAsync(formationId));
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentDto>> Create(CreateEquipmentDto dto)
    {
        var result = await equipmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EquipmentDto>> Update(int id, UpdateEquipmentDto dto)
    {
        return Ok(await equipmentService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await equipmentService.DeleteAsync(id);
        return NoContent();
    }
}
