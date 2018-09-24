using System;
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
    }
}