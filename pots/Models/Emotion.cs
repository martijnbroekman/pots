using System;

namespace pots.Models
{
    public class Emotion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Time { get; set; }
        public decimal Angry { get; set; }
        public decimal Disgusted { get; set; }
        public decimal Fearful { get; set; }
        public decimal Happy { get; set; }
        public decimal Sad { get; set; }
        public decimal Surprised { get; set; }
        public decimal Neutral { get; set; }
    }
}