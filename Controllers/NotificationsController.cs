using Gochs.Nfgo.DTOs.Common;
using Gochs.Nfgo.DTOs.Notifications;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Управление оповещениями формирований НФГО.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    /// <summary>
    /// Получить историю всех оповещений.
    /// </summary>
    /// <response code="200">История оповещений успешно получена.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDto>>> GetAll()
    {
        return Ok(await notificationService.GetAllAsync());
    }

    /// <summary>
    /// Получить оповещение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор оповещения.</param>
    /// <response code="200">Оповещение найдено.</response>
    /// <response code="404">Оповещение не найдено.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationDto>> GetById(int id)
    {
        return Ok(await notificationService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить оповещения конкретного формирования.
    /// </summary>
    /// <param name="formationId">Идентификатор формирования.</param>
    /// <response code="200">История оповещений успешно получена.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpGet("/api/formations/{formationId:int}/notifications")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDto>>> GetByFormation(int formationId)
    {
        return Ok(await notificationService.GetByFormationIdAsync(formationId));
    }

    /// <summary>
    /// Зарегистрировать новое оповещение.
    /// </summary>
    /// <param name="dto">Данные оповещения.</param>
    /// <response code="201">Оповещение создано в статусе Created.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Указанное формирование не найдено.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificationDto>> Create(CreateNotificationDto dto)
    {
        var result = await notificationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Изменить статус оповещения.
    /// </summary>
    /// <remarks>
    /// Допустимы переходы только из Created в Sent или Failed.
    /// </remarks>
    /// <param name="id">Идентификатор оповещения.</param>
    /// <param name="dto">Новый статус оповещения.</param>
    /// <response code="200">Статус обновлён.</response>
    /// <response code="400">Передан некорректный статус.</response>
    /// <response code="404">Оповещение не найдено.</response>
    /// <response code="409">Переход статуса запрещён бизнес-правилами.</response>
    [HttpPatch("{id:int}/status")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<NotificationDto>> UpdateStatus(int id, UpdateNotificationStatusDto dto)
    {
        return Ok(await notificationService.UpdateStatusAsync(id, dto));
    }
}
