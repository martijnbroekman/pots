using System;

namespace pots.Models
{
    public class Step
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}