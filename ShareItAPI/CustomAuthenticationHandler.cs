using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ShareItAPI.Models;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;

namespace ShareItAPI
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ShareItDBContext _db;
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ShareItDBContext db,
            AuthSettings authSettings) : base(options, logger, encoder, clock)
        {
            _db = db;
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeaders = Request.Headers.Authorization;
            var sessionKey = authorizationHeaders.FirstOrDefault();
            
            var session = await _db.Sessions.Where(s => s.Key == sessionKey).Include(s => s.User).FirstOrDefaultAsync();

            if (session is not null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, session.User.Username),
                };

                AuthenticationTicket ticket = new AuthenticationTicket(
                        new ClaimsPrincipal(new ClaimsIdentity(claims, "SESSION")),
                         this.Scheme.Name
                        );
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.NoResult();
        }
    }
}
