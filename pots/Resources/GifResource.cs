namespace pots.Resources
{
    public class CreateGifResource
    {
        public string GifUrl { get; set; }
    }
    
    public class GifResource : CreateGifResource
    {
        public int Id { get; set; }
    } 
}