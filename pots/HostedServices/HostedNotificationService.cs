using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using pots.Data;
using pots.Hubs;

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

        public async void DoWork()
        {
            await _hubContext.Clients.All.SendAsync("hallo plebs");
        }
    }
}