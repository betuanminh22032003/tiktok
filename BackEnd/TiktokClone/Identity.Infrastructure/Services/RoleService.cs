using Identity.Application.Interfaces;

namespace Identity.Infrastructure.Services
{
    /// <summary>
    /// Demo in-memory role store. Replace with DB-backed implementation in production.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IDictionary<string, string> _roles = new Dictionary<string, string>();

        public void AssignRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role)) return;
            lock (_roles)
            {
                _roles[userId] = role;
            }
        }

        public string GetRole(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return "User";
            lock (_roles)
            {
                if (_roles.TryGetValue(userId, out var role)) return role;
            }
            return "User";
        }
    }
}
