using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;
using BCrypt.Net;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            _logger.LogInformation("Attempting to register user {Username}", registerDto.Username);

            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed for {Username}: Username already exists.", registerDto.Username);
                return Conflict("Username already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            await _userRepository.CreateUserAsync(user);
            _logger.LogInformation("User {Username} registered successfully.", user.Username);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, "User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            _logger.LogInformation("Attempting to login user {Username}", loginDto.Username);

            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for {Username}: Invalid credentials.", loginDto.Username);
                return Unauthorized("Invalid credentials.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Username);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // In a real app, you'd save the refresh token to the database and associate it with the user.
            // For now, we'll just return it.

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Should be true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            
            _logger.LogInformation("User {Username} logged in successfully.", user.Username);

            return Ok(new { AccessToken = accessToken });
        }
    }
}
