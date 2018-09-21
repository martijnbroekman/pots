namespace pots.Models
{
    public class UserInNotification
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Notification Notification { get; set; }
        public int NotificationId { get; set; }
        public bool Accepted { get; set; }
    }
}