namespace ShareItAPI.DTO
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
