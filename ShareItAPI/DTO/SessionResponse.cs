using ShareItAPI.Models;

namespace ShareItAPI.DTO
{
    public class SessionResponse
    {
        public string Key { get; set; } = null!;
        public virtual UserResponse User { get; set; } = null!;
    }
}
