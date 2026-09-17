using ControllersInterfaces.Sessions;
using ModelsAPI.Sessions;
using ServicesInterfaces.Sessions;

namespace Controllers.Sessions;

public class SessionController : ISessionController
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public ResponseCreateSession CreateSession(RequestCreateSession request)
    {
        var session = _sessionService.Login(request.Email, request.Password);
        
        return new ResponseCreateSession
        {
            Token = session.Token
        };
    }

    public void DeleteSession(string token)
    {
        _sessionService.Logout(token);
    }
}