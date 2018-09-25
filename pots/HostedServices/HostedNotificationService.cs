using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using pots.Data;
using pots.Hubs;
using pots.Models;
using pots.Resources;
using pots.Helpers;

namespace pots.HostedServices
{
    public class HostedNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public HostedNotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task DoWork()
        {
            await SendCompetitiveNotifications();
            await SendSocialNotifications();
        }

        private async Task SendCompetitiveNotifications()
        {
            var users = await _context.Users.Where(u => u.CanReceiveNotification && u.Type.Equals(NotificationType.Competence)).ToListAsync();
            var activities = await _context.Activities.Where(a => a.Type.Equals(NotificationType.Competence)).ToListAsync();

            await SendNotifications(users, activities);
        }
        
        private async Task SendSocialNotifications()
        {
            var users = await _context.Users.Where(u => u.CanReceiveNotification && u.Type.Equals(NotificationType.Relatedness)).ToListAsync();
            var activities = await _context.Activities.Where(a => a.Type.Equals(NotificationType.Relatedness)).ToListAsync();

            await SendNotifications(users, activities);
        }

        private async Task SendNotifications(List<User> users, List<Activity> activities)
        {
            
            if (!activities.Any() || !users.Any())
                return;
            
            var rnd = new Random();
            users = users.Shuffle(rnd).ToList();
            activities = activities.Shuffle(rnd).ToList();
            
            var notifications = new List<Notification>();

            var currentNotification = GetNotification(activities);
            notifications.Add(currentNotification);
            _context.Add(currentNotification);
            await _context.SaveChangesAsync();

            var totalUsers = users.Count;
            foreach (var item in users.Select((value, i) => new { i, value }))
            {
                var user = item.value;
                var index = item.i;
                var usersLeft = totalUsers - index;
                
                if (currentNotification.Users.Count == currentNotification.Activity.AmountOfUsers)
                {
                    // Saving the users in the notifications
                    await _context.SaveChangesAsync();
                    
                    if (!activities.Any())
                    {
                        currentNotification = null;
                        break;
                    }
                    currentNotification = GetNotification(activities);
                    
                    if (usersLeft < currentNotification.Activity.AmountOfUsers && currentNotification.Activity.CanBeLess)
                        break;

                    _context.Add(currentNotification);
                    await _context.SaveChangesAsync();
                    notifications.Add(currentNotification);
                }

                var userInNotification = new UserInNotification
                {
                    User = user,
                    UserId = user.Id,
                    Accepted = false,
                    Notification = currentNotification,
                    NotificationId = currentNotification.Id
                };
                currentNotification.Users.Add(userInNotification);
                _context.Add(userInNotification);
            }

            await _context.SaveChangesAsync();

            if (currentNotification != null && (currentNotification.Users.Count <= 1 || (!currentNotification.Activity.CanBeLess && currentNotification.Users.Count < currentNotification.Activity.AmountOfUsers)))
            {
                _context.RemoveRange(currentNotification.Users);
                await _context.SaveChangesAsync();
                
                _context.Remove(currentNotification);
                await _context.SaveChangesAsync();
                notifications.Remove(currentNotification);
            }

            foreach (var notification in notifications)
            {
                foreach (var user in notification.Users)
                {
                    var otherUsers = notification.Users.Where(u => u != user).Select(u => u.User.Name);
                    await _hubContext.Clients.User(user.UserId.ToString()).SendAsync("notification", new
                    {
                        Id = notification.Id,
                        Description = string.Format(notification.Activity.Description, string.Join(',', otherUsers)),
                        AmountOfUsers = notification.Activity.AmountOfUsers,
                        CanBeLess = notification.Activity.CanBeLess
                    });
                }
            }
        }

        private Notification GetNotification(IList<Activity> activities)
        {
            var activity = activities.FirstOrDefault();
            activities.RemoveAt(0);
            
            return new Notification
            {
                Activity = activity,
                TimeSend = DateTime.Now,
                Users = new List<UserInNotification>()
            };
        }
    }
}