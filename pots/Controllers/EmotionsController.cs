using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pots.Data;
using pots.Resources;

namespace pots.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EmotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public EmotionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> Insert(EmotionResource resource)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(resource.UserId));
            if (user == null)
                return NotFound(nameof(user));
            
            var emotion = resource.GetModelFromResource();
            emotion.Time = DateTime.Now;
            
            _context.Add(emotion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetForUser(int userId)
        {
            var user = await _context.Users.Include(u => u.Emotions).FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if (user == null)
                return NotFound(nameof(user));

            return Ok(user.Emotions.Select(EmotionResource.GetEmotionResource));
        }
    }
}