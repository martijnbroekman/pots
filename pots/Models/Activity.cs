using System.Collections.Generic;

namespace pots.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int AmountOfUsers { get; set; }
        public bool CanBeLess { get; set; }
        public NotificationType Type { get; set; }
        public List<Gif> Gifs { get; set; }
    }
}