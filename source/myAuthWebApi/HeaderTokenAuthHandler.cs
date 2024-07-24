using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace myAuthWebApi
{

    public class HeaderTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HeaderTokenAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
            {
            }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Header-Token"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authHeader = Request.Headers["Header-Token"].ToString();
            if (authHeader != "your_password")
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "user_id"),
            new Claim(ClaimTypes.Name, "mike"),
        };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
