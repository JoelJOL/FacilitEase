using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAngularDev")]
public class NotificationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        try
        {
            // Use the Notification repository from Unit of Work
            var notifications = await _unitOfWork.Notification.GetNotificationsByUserIdAsync(userId);

            return Ok(notifications);
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately (e.g., log them)
            return StatusCode(500, "Internal Server Error");
        }
    }
}