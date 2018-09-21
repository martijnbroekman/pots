using System.Collections.Generic;

namespace pots.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanReceiveNotification { get; set; }
        public List<Step> Step { get; set; }
        public List<Emotion> Emotions { get; set; }
    }
}
