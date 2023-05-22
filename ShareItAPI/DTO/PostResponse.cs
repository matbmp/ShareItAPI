using ShareItAPI.Models;

namespace ShareItAPI.DTO
{
    public class PostResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public long Likes { get; set; }
        public bool? IsLiked { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserResponse Author { get; set; } = null!;
        public virtual List<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
        public virtual SectionResponse Section { get; set; } = null!;
    }
}
