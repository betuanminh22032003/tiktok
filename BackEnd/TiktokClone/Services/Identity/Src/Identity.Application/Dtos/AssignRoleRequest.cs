namespace Identity.Application.Dtos
{
    public class AssignRoleRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
