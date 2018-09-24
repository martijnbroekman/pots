using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using pots.Data;

namespace pots.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext _context;
        
        public NotificationHub(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public override Task OnConnectedAsync()
        {
            var test = GetUserId();
            
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            return Context.User.Claims.First(c => c.Type == "sub").Value;
        }
    }
}