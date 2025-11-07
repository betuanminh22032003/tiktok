using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Controllers;
using UserService.Models;
using UserService.Repositories;
using System.Threading.Tasks;
using UserService.DTOs;
using UserService.Services;
using Microsoft.AspNetCore.Http;

namespace UserService.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockRepo.Object, _mockTokenService.Object, _mockLogger.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Register_ReturnsConflict_WhenUsernameIsTaken()
        {
            // Arrange
            var registerDto = new UserRegisterDto { Username = "takenuser", Password = "password", Email = "a@a.com" };
            _mockRepo.Setup(repo => repo.GetUserByUsernameAsync(registerDto.Username))
                .ReturnsAsync(new User());

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Username already exists.", conflictResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsCreatedAtAction_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = new UserRegisterDto { Username = "newuser", Password = "password", Email = "a@a.com" };
            _mockRepo.Setup(repo => repo.GetUserByUsernameAsync(registerDto.Username))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("User registered successfully.", createdAtActionResult.Value);
        }
    }
}
