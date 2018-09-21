namespace pots.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int AmountOfUsers { get; set; }
        public bool CanBeLess { get; set; }
        public NotificationType Type { get; set; }
    }
}