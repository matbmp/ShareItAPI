namespace ShareItAPI.Models
{
    public class PostLike
    {
        public long PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
