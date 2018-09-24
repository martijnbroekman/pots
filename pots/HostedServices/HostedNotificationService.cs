using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using pots.Data;
using pots.Hubs;
using pots.Models;
using pots.Resources;

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
            var users = await _context.Users.Where(u => u.CanReceiveNotification).ToListAsync();
            var activities = await _context.Activities.ToListAsync();
            
            var rnd = new Random();
            
            var r = rnd.Next(activities.Count);
            var rand = activities[r];
            activities.Remove(rand);
            
            foreach (var user in users)
            {
                await _hubContext.Clients.User(user.Id.ToString()).SendAsync("notification", new ActivityResource
                {
                    Id = 1,
                    Description = "test",
                    AmountOfUsers = 2,
                    CanBeLess = false,
                    Type = "Competence"
                });
            }
        }

        private async Task SendCompetitiveNotifications()
        {
            var users = await _context.Users.Where(u => u.CanReceiveNotification && u.Type.Equals(NotificationType.Competence)).OrderBy(x => Guid.NewGuid()).ToListAsync();
            var activities = await _context.Activities.Where(a => a.Type.Equals(NotificationType.Competence)).OrderBy(x => Guid.NewGuid()).ToListAsync();
            
            var notifications = new List<Notification>();

            var currentNotification = GetNotification(activities);
            
            foreach (var user in users)
            {
                if (currentNotification.Users.Count == currentNotification.Activity.AmountOfUsers)
                {
                    notifications.Add(currentNotification);
                    if (!activities.Any())
                        break;
                    currentNotification = GetNotification(activities);
                }

                currentNotification.Users.Add(new UserInNotification
                {
                    User = user,
                    UserId = user.Id,
                    Accepted = false,
                    Notification = currentNotification
                });
            }

            foreach (var notification in notifications)
            {
                foreach (var user in notification.Users)
                {
                    await _hubContext.Clients.User(user.Id.ToString()).SendAsync("notification", new ActivityResource
                    {
                        Id = 1,
                        Description = "test",
                        AmountOfUsers = 2,
                        CanBeLess = false,
                        Type = "Competence"
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
        
        private async Task SendSocialNotifications()
        {
            
        }
    }
}