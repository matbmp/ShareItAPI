namespace ShareItAPI.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set;}
        public long AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public long? SectionId { get; set; }
        public virtual Section? Section { get; set; }
        public virtual List<User> UserFeed { get; set; } = new List<User>();

        public long Likes { get; set; }
        public virtual ICollection<PostLike> UserLikes { get; set; } = new List<PostLike>();
    }
}
