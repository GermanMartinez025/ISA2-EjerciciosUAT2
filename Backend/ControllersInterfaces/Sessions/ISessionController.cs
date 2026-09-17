using ModelsAPI.Sessions;

namespace ControllersInterfaces.Sessions;

public interface ISessionController
{
    ResponseCreateSession CreateSession(RequestCreateSession request);
    void DeleteSession(string token);
}