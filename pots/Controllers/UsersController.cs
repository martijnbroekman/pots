using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pots.Data;
using pots.Models;
using pots.Resources;

namespace pots.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        
        public UsersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users.Select(UserResource.GetUserResource));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserResource user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                return BadRequest(nameof(user.Name));
            if (string.IsNullOrWhiteSpace(user.Mail))
                return BadRequest(nameof(user.Mail));
            if (string.IsNullOrWhiteSpace(user.Password))
                return BadRequest(nameof(user.Password));
            
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Mail,
                NormalizedEmail = user.Mail.ToUpper(),
                UserName = user.Mail,
                NormalizedUserName = user.Mail.ToUpper(),
                Type = user.Type
            };
            await _userManager.CreateAsync(newUser, user.Password);
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