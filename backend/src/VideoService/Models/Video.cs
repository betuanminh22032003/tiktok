namespace VideoService.Models
{
    public class Video
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
