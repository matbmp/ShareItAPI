namespace ShareItAPI.DTO
{
    public class PostPostRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
