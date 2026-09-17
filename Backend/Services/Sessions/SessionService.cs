using IRepositories.Repositories.SessionRepositories;
using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Sessions;
using ModelInterface.Users;
using Models.Sessions;
using Models.Users;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace Services.Sessions;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserService _userService;
    private AUser? _user;
    
    public SessionService(ISessionRepository sessionRepository, IUserService userService)
    {
        _sessionRepository = sessionRepository;
        _userService = userService;
    }
    
    
    public ISession Login(string email, string password)
    {
        _userService.ValidateCredentials(email, password);
        
        var user = _userService.GetByEmail(email);
        
        var session = new Session(user);
        
        ValidateSession(session);
        
        _sessionRepository.Create(session);
        
        return session;
    }

    public AUser GetUserFromSession(string token)
    {
      ISession session = _sessionRepository.GetByToken(token);
      
      ValidateSession(session);
      
      return _userService.GetById(session.UserId);
    }
    
    public int GetUserIdFromSession(string token)
    {
        AUser user = GetUserFromToken(token);
        
        return user.Id;
    }

    public bool ValidSession(string token, string role)
    {
        if (_user == null) _user = GetUserFromToken(token);
        
        return _user.HasRole(role);
    }
    
    public bool ValidSession(string token, List<string> roles)
    {
        return roles.Any(role => ValidSession(token, role));
    }

    public void Logout(string token)
    {
        var session = _sessionRepository.GetByToken(token);
        
        _sessionRepository.Delete(session);
    }

    public List<string> GetRolesFromSession(string? token)
    {
        var role = new List<string>();
        if (String.IsNullOrEmpty(token))
        {
            role.Add("NoSession");
        }
        else
        {
            AUser user = GetUserFromToken(token);
            role = user.GetRoles();
        }
        
        return role;
    }
    
    private AUser GetUserFromToken(string token)
    {
        var session = _sessionRepository.GetByToken(token);
        
        ValidateSession(session);
        
        var user = _userService.GetById(session.UserId);
        
        return user;
    }
    
    private void ValidateSession(ISession session)
    {
        if (session == null) throw new NotFoundException("Session not found");
    }
}