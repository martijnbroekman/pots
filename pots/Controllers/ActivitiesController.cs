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
            return Ok((await _context.Activities.ToListAsync()).Select(ActivityResource.GetActivityResource));
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
    }
}