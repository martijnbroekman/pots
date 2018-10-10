namespace pots.Models
{
    public class Gif
    {
        public int Id { get; set; }
        public string GifUrl { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}