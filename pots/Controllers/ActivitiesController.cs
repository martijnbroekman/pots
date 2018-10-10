using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using pots.Data;
using pots.Models;
using pots.Resources;

namespace pots.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await _context.Activities.Include(a => a.Gifs).ToListAsync()).Select(ActivityResource.GetActivityResource));
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (activity == null)
                return NotFound(nameof(activity));

            return Ok(ActivityResource.GetActivityResource(activity));
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ActivityResource resource)
        {
            var activity = resource.GetModelFromResource();
            _context.Add(activity);
            await _context.SaveChangesAsync();

            return Ok(ActivityResource.GetActivityResource(activity));
        }

        [HttpPatch("{id}/gifs")]
        public async Task<IActionResult> AddNotification(int id, [FromBody]CreateGifResource resource)
        {
            var gif = new Gif
            {
                GifUrl = resource.GifUrl,
                ActivityId = id
            };

            _context.Add(gif);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var activity = await _context.Activities.Include(a => a.Gifs).FirstOrDefaultAsync(a => a.Id.Equals(id));
            
            if (activity == null)
                return NotFound(nameof(activity));

            _context.RemoveRange(activity.Gifs);
            await _context.SaveChangesAsync();
            
            _context.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}