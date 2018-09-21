using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pots.Data;
using pots.Models;
using pots.Resources;

namespace pots.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users.Select(UserResource.GetUserResource));
        }

        [HttpPost]
        public async Task<IActionResult> GetNew([FromBody]CreateUserResource user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                return BadRequest(nameof(user.Name));
            
            var newUser = new User
            {
                Name = user.Name
            };
            _context.Add(newUser);
            await _context.SaveChangesAsync();
            
            return Ok(new { Id = newUser.Id });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCanReceiveNotification(int id, [FromBody]UserResource resource)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (user == null)
                return NotFound(nameof(user));

            if (user.CanReceiveNotification != resource.CanReceiveNotification)
            {
                user.CanReceiveNotification = resource.CanReceiveNotification;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}