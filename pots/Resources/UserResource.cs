using pots.Models;

namespace pots.Resources
{
    public class CreateUserResource
    {
        public string Name { get; set; }
    }
    
    public class UserResource : CreateUserResource
    {
        public int Id { get; set; }
        public bool CanReceiveNotification { get; set; }

        public static UserResource GetUserResource(User user)
        {
            return new UserResource
            {
                Id = user.Id,
                Name = user.Name,
                CanReceiveNotification = user.CanReceiveNotification
            };
        }
    }
}