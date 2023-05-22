namespace ShareItAPI.Models
{
    public class CommentLike
    {
        public long CommentId { get; set; }
        public virtual Comment Comment { get; set; } = null!;
        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
