namespace Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; } = "User";

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public void SetRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return;
            Role = role;
        }
    }

}
