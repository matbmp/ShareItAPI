namespace ShareItAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual List<Section> Following { get; set; } = new List<Section>();
        public virtual List<Post> CreatedPosts { get; set; } = new List<Post>();
        public virtual List<Comment> CreatedComments { get; set; } = new List<Comment>();
        public virtual List<Post> FeedPosts { get; set; } = new List<Post>();
        public virtual List<Session> Sessions { get; set; } = new List<Session>();

        public virtual ICollection<PostLike> LikedPosts { get; set; } = new List<PostLike>();
        public virtual List<CommentLike> LikedComments { get; set; } = new List<CommentLike>();
    }
}
