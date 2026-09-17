using ModelsAPI.Users;

namespace ControllersInterfaces.Users;

public interface IUserController
{
    public ResponseGetUser AddUser(RequestAddUser requestAddUser, string? token);
    
    public ResponseDeleteUser DeleteUser(int id, string token);
    
    public ResponseGetUsers GetUsers(int page, int pageSize, List<string>? roles, List<string>? names);

    public ResponseGetUser UpdateUser(RequestUpdateUser requestUpdateUser, string token);
}