using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using pots.Data;
using pots.Hubs;
using pots.Models;
using pots.Resources;

namespace pots.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPatch("{id}/accept")]
        public async Task<IActionResult> Update(int id, AcceptNotificationResource resource)
        {
            var userIdString = User?.FindFirst("sub")?.Value;
            if (userIdString == null)
                return Unauthorized();
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            var notification = await _context.Notifications
                .Include(n => n.Activity)
                .Include(n => n.Users)
                .ThenInclude(uin => uin.User)
                .FirstOrDefaultAsync(n => n.Id.Equals(id));

            if (notification == null)
                return NotFound($"No notification with the ID: {id}");
            
            var userInNotification = notification.Users.FirstOrDefault(u => u.UserId.Equals(userId));
            
            if (userInNotification == null)
                return NotFound($"No user in notification found with the user ID: {userId}");

            
            userInNotification.Accepted = resource.Accept;
            userInNotification.ResponseTime = DateTime.Now;
            _context.Update(userInNotification);
            await _context.SaveChangesAsync();

            if (!resource.Accept)
            {
                await _hubContext.Clients
                    .Users(notification.Users.Where(u => u.UserId != userId).Select(u => u.UserId.ToString()).ToList())
                    .SendAsync("notification:decline", $"{userInNotification.User.Name} durft niet");
            }
            else if (notification.Users.All(u => u.Accepted))
            {
                foreach (var user in notification.Users)
                {
                    var otherUserNames = notification.Users.Where(u => !u.UserId.Equals(user.UserId)).Select(u => u.User.Name);
                    var otherUserNamesString = string.Join(", ", otherUserNames);
                    
                    await _hubContext.Clients.Users(user.UserId.ToString())
                        .SendAsync("notification:accept", new 
                        {
                            Title = notification.Activity.Type.Equals(NotificationType.Competence) ? "LET'S GO!" : "Have fun!",
                            Text = notification.Activity.Type.Equals(NotificationType.Competence) ? 
                                $"Maak je klaar voor de strijd!" :
                                $"Even lekker ontspannen samen met {otherUserNamesString}"
                        });
                }
            }
            
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
            
            if (notifications == null || !notifications.Any())
                return NotFound($"No notifications for the user ID: {userId}");

            return Ok(notifications);
        }
    }
}