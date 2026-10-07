using Gochs.Nfgo.DTOs.Common;
using Gochs.Nfgo.DTOs.Equipment;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Управление техникой и имуществом подразделений НФГО.
/// </summary>
[ApiController]
[Route("api/equipment")]
[Produces("application/json")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        this.equipmentService = equipmentService;
    }

    /// <summary>
    /// Получить всю технику и имущество.
    /// </summary>
    /// <response code="200">Список техники успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetAll()
    {
        return Ok(await equipmentService.GetAllAsync());
    }

    /// <summary>
    /// Получить позицию техники или имущества по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор позиции.</param>
    /// <response code="200">Позиция найдена.</response>
    /// <response code="404">Позиция не найдена.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipmentDto>> GetById(int id)
    {
        return Ok(await equipmentService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить технику конкретного подразделения.
    /// </summary>
    /// <param name="unitId">Идентификатор подразделения.</param>
    /// <response code="200">Список техники успешно получен.</response>
    /// <response code="404">Подразделение не найдено.</response>
    [HttpGet("/api/units/{unitId:int}/equipment")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetByUnit(int unitId)
    {
        return Ok(await equipmentService.GetByUnitIdAsync(unitId));
    }

    /// <summary>
    /// Получить технику конкретного формирования.
    /// </summary>
    /// <param name="formationId">Идентификатор формирования.</param>
    /// <response code="200">Список техники успешно получен.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpGet("/api/formations/{formationId:int}/equipment")]
    [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<EquipmentDto>>> GetByFormation(int formationId)
    {
        return Ok(await equipmentService.GetByFormationIdAsync(formationId));
    }

    /// <summary>
    /// Добавить технику или имущество.
    /// </summary>
    /// <param name="dto">Данные новой позиции.</param>
    /// <response code="201">Позиция создана.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Указанное подразделение не найдено.</response>
    /// <response code="409">Инвентарный номер уже используется.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EquipmentDto>> Create(CreateEquipmentDto dto)
    {
        var result = await equipmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить технику или имущество.
    /// </summary>
    /// <param name="id">Идентификатор позиции.</param>
    /// <param name="dto">Новые данные позиции.</param>
    /// <response code="200">Позиция обновлена.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Позиция или указанное подразделение не найдены.</response>
    /// <response code="409">Инвентарный номер уже используется.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EquipmentDto>> Update(int id, UpdateEquipmentDto dto)
    {
        return Ok(await equipmentService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Удалить технику или имущество.
    /// </summary>
    /// <param name="id">Идентификатор позиции.</param>
    /// <response code="204">Позиция удалена.</response>
    /// <response code="404">Позиция не найдена.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await equipmentService.DeleteAsync(id);
        return NoContent();
    }
}
