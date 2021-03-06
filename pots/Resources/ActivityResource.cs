using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using pots.Models;

namespace pots.Resources
{
    public class ActivityResource
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int AmountOfUsers { get; set; }
        public bool CanBeLess { get; set; }
        public string Type { get; set; }
        public List<GifResource> Gifs { get; set; }

        public static ActivityResource GetActivityResource(Activity activity)
        {
            return new ActivityResource
            {
                Id = activity.Id,
                Description = activity.Description,
                AmountOfUsers = activity.AmountOfUsers,
                CanBeLess = activity.CanBeLess,
                Type = activity.Type.ToString(),
                Gifs = activity.Gifs != null && activity.Gifs.Any() ? activity.Gifs.Select(g => new GifResource
                    {
                        GifUrl = g.GifUrl,
                        Id = g.Id
                        
                    }
                ).ToList() : new List<GifResource>()
            };
        }

        public Activity GetModelFromResource()
        {
            return new Activity
            {
                Id = Id,
                Description = Description,
                AmountOfUsers = AmountOfUsers,
                CanBeLess = CanBeLess,
                Type = (NotificationType)System.Enum.Parse(typeof(NotificationType), Type, true)
            };
        }
    }
}