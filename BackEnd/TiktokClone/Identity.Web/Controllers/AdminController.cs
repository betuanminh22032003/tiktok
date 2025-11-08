using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public AdminController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Protected by role-based authorization
        [Authorize(Roles = "Admin")]
        [HttpGet("secret")]
        public IActionResult Secret()
        {
            return Ok(new { message = "This is an admin-only secret." });
        }

        // Assign a role to a user (admin-only)
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public IActionResult AssignRole([FromBody] AssignRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("UserId and Role are required.");

            _roleService.AssignRole(request.UserId, request.Role);
            return NoContent();
        }
    }
}
