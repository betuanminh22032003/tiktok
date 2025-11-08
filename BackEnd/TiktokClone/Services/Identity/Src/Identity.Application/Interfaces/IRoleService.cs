namespace Identity.Application.Interfaces
{
    public interface IRoleService
    {
        void AssignRole(string userId, string role);
        string GetRole(string userId);
    }
}
