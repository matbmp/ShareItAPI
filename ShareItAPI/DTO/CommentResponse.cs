using ShareItAPI.Models;

namespace ShareItAPI.DTO
{
    public class CommentResponse
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public bool? IsLiked { get; set; }
        public long Likes { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserResponse Author { get; set; } = null!;
    }
}
