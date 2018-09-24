using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace pots.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public bool CanReceiveNotification { get; set; }
        public List<Step> Step { get; set; }
        public List<Emotion> Emotions { get; set; }
        public NotificationType Type { get; set; }
    }
}
