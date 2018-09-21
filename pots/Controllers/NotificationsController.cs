using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pots.Data;

namespace pots.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPatch("{id}/users/{userId}/accept")]
        public async Task<IActionResult> Update(int id, int userId)
        {
            var userInNotification = await _context.UserInNotifications.FirstOrDefaultAsync(uin =>  uin.NotificationId.Equals(id) && uin.UserId.Equals(userId));
            if (userInNotification == null)
                return NotFound($"No notification with the ID: {id} and user ID: {userId}");

            userInNotification.Accepted = true;
            _context.Update(userInNotification);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(int userId)
        {
            var notifications = await _context.UserInNotifications
                .Include(uin => uin.Notification)
                .ThenInclude(n => n.Activity)
                .Include(uin => uin.Notification)
                .ThenInclude(n => n.Users)
                .Where(uin => uin.UserId.Equals(userId))
                .Select(uin => uin.Notification).ToListAsync();
            
            if (notifications == null && !notifications.Any())
                return NotFound($"No notifications for the user ID: {userId}");

            return Ok(notifications);
        }
    }
}