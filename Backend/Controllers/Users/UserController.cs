using ControllersInterfaces.Users;
using ServicesInterfaces.Users;
using Mapper;
using ModelException;
using ModelInterface.Users;
using ServicesInterfaces.Sessions;
using ModelsAPI.Users;
using Services.Users.FabricUsers;

namespace Controllers.Users;

public class UserController : IUserController
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    
    public UserController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService;
    }
    
    public ResponseGetUser AddUser(RequestAddUser requestAddUser, string? token)
    {
        var roles = _sessionService.GetRolesFromSession(token);
        
        var user = _userService.AddUser(requestAddUser, roles);
        return new ResponseGetUser(user);
    }
    public ResponseDeleteUser DeleteUser(int id, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        if (user.Id == id)
        {
            throw new ForbiddenException("Can't delete yourself");
        }
        _userService.DeleteUser(id);
        return new ResponseDeleteUser(id);
    }
    
    public ResponseGetUsers GetUsers(int page, int pageSize, List<string>? roles, List<string>? names)
    {
        Dictionary<string, object> filtersDictionary = new Dictionary<string, object>();
        if (names != null) filtersDictionary.Add("fullName", names);
        if (roles != null) filtersDictionary.Add("role", roles);
        
        var users = _userService.GetUsers(page, pageSize, filtersDictionary);
        
        var responseUsers = MapUsers(users);   
        responseUsers.TotalUsers = _userService.GetAmountOfUsers(filtersDictionary);
        responseUsers.actualPage = page;
        responseUsers.totalPages = (int)Math.Ceiling((double)responseUsers.TotalUsers / pageSize);
        
        return responseUsers;
    }

    public ResponseGetUser UpdateUser(RequestUpdateUser requestUpdateUser, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var updated = _userService.UpdateUser(user.Id, requestUpdateUser);
        return new ResponseGetUser(updated);
    }

    private ResponseGetUsers MapUsers(List<AUser> users)
    {
        var responseUsers = new ResponseGetUsers();
        var mapperUser = new Mapper<AUser, ResponseGetUser>();
        
        foreach (var user in users)
        {
            responseUsers.Users.Add(mapperUser.Convert(user));
        }
        
        return responseUsers;
    }
    
    
}