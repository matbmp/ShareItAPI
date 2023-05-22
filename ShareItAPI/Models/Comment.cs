namespace ShareItAPI.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public long AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual User Author { get; set; } = null!;
        public long PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        public long Likes { get; set; }
        public virtual ICollection<CommentLike> UserLikes { get; set; } = new List<CommentLike>();
    }
}
