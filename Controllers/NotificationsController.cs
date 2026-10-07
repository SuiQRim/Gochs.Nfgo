using Gochs.Nfgo.DTOs.Notifications;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDto>>> GetAll()
    {
        return Ok(await notificationService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NotificationDto>> GetById(int id)
    {
        return Ok(await notificationService.GetByIdAsync(id));
    }

    [HttpGet("/api/formations/{formationId:int}/notifications")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDto>>> GetByFormation(int formationId)
    {
        return Ok(await notificationService.GetByFormationIdAsync(formationId));
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> Create(CreateNotificationDto dto)
    {
        var result = await notificationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<NotificationDto>> UpdateStatus(int id, UpdateNotificationStatusDto dto)
    {
        return Ok(await notificationService.UpdateStatusAsync(id, dto));
    }
}
