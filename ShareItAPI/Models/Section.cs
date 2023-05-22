namespace ShareItAPI.Models
{
    public class Section
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
