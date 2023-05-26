using Microsoft.EntityFrameworkCore;
using ShareItAPI.Models;
using System.Security.Claims;

namespace ShareItAPI.Services
{
    public class AuthorizationService
    {
        private IHttpContextAccessor contextAccessor;
        private ShareItDBContext db;
        public AuthorizationService(IHttpContextAccessor contextAccessor, ShareItDBContext db)
        {
            this.contextAccessor = contextAccessor;
            this.db = db;
        }

        public async Task<User?> getLoggedUserAsync()
        {
            if (contextAccessor.HttpContext is not HttpContext context) return null;
            if (context.User.Identity is not ClaimsIdentity identity) return null;
            if (identity.FindFirst(ClaimTypes.Name) is not Claim nameClaim) return null;
            return await db.Users.ByUsername(nameClaim.Value).FirstOrDefaultAsync();
        }
    }
}
