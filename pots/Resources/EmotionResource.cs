using pots.Models;

namespace pots.Resources
{
    public class EmotionResource
    {
        public decimal Angry { get; set; }
        public decimal Disgusted { get; set; }
        public decimal Fearful { get; set; }
        public decimal Happy { get; set; }
        public decimal Sad { get; set; }
        public decimal Surprised { get; set; }
        public decimal Neutral { get; set; }
        public int UserId { get; set; }

        public Emotion GetModelFromResource()
        {
            return new Emotion
            {
                Angry = Angry,
                Disgusted = Disgusted,
                Fearful = Fearful,
                Happy = Happy,
                Sad = Sad,
                Surprised = Surprised,
                Neutral = Neutral,
                UserId = UserId
            };
        }

        public static EmotionResource GetEmotionResource(Emotion emotion)
        {
            return new EmotionResource
            {
                Angry = emotion.Angry,
                Disgusted = emotion.Disgusted,
                Fearful = emotion.Fearful,
                Happy = emotion.Happy,
                Sad = emotion.Sad,
                Surprised = emotion.Surprised,
                Neutral = emotion.Neutral,
                UserId = emotion.UserId
            };
        }
    }
}