using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IAuthService authService, Microsoft.Extensions.Options.IOptions<JwtSettings> options)
        {
            _authService = authService;
            _jwtSettings = options.Value;
        }

        // Demo register - in a real app you'd persist the user and validate credentials
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRequest request)
        {
            // For demo create a user id from username
            var user = new { Id = Guid.NewGuid().ToString(), Username = request.Username };
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = _authService.Login(request.Username, request.Password);

                var accessCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.AccessTokenExpiresAt
                };

                var refreshCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.RefreshTokenExpiresAt
                };

                Response.Cookies.Append("access_token", result.AccessToken, accessCookieOptions);
                Response.Cookies.Append("refresh_token", result.RefreshToken, refreshCookieOptions);

                return Ok(new { result.UserId, result.Username });
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid credentials");
            }
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var oldRefresh))
                return Unauthorized();

            try
            {
                var result = _authService.Refresh(oldRefresh);

                var accessCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.AccessTokenExpiresAt
                };

                var refreshCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.RefreshTokenExpiresAt
                };

                Response.Cookies.Append("access_token", result.AccessToken, accessCookieOptions);
                Response.Cookies.Append("refresh_token", result.RefreshToken, refreshCookieOptions);

                return Ok();
            }
            catch (System.Security.SecurityException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refresh))
            {
                _authService.Logout(refresh);
            }

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var sub = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
            var name = User.Identity?.Name ?? User.FindFirst("unique_name")?.Value ?? string.Empty;
            return Ok(new { Id = sub, Username = name });
        }
    }
}
