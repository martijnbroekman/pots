using System;
using System.Collections.Generic;

namespace pots.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime TimeSend { get; set; }
        public Activity Activity { get; set; }
        public List<UserInNotification> Users { get; set; }
    }
}