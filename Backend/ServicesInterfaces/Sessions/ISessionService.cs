using ModelInterface.Sessions;
using ModelInterface.Users;
using Models.Users;

namespace ServicesInterfaces.Sessions;

public interface ISessionService
{
    ISession Login(string email, string password);
    AUser GetUserFromSession(string token);
    bool ValidSession(string token, string role);
    bool ValidSession(string token, List<string> roles);
    void Logout(string token);
    List<string> GetRolesFromSession(string? token);
    public int GetUserIdFromSession(string token);
}