using pots.Models;

namespace pots.Resources
{
    public class BaseUserResource
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public NotificationType Type { get; set; }
    }
    
    public class CreateUserResource : BaseUserResource
    {
        public string Password { get; set; }
    }
    
    public class UserResource : BaseUserResource
    {
        public int Id { get; set; }
        public bool CanReceiveNotification { get; set; }

        public static UserResource GetUserResource(User user)
        {
            return new UserResource
            {
                Id = user.Id,
                Name = user.Name,
                Mail = user.Email,
                CanReceiveNotification = user.CanReceiveNotification,
                Type = user.Type
            };
        }
    }
}