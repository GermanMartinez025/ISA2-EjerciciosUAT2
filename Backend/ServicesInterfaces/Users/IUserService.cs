using ModelInterface.Users;
using Models.Users;
using ModelsAPI.Users;

namespace ServicesInterfaces.Users;

public interface IUserService
{
    public AUser AddUser(RequestAddUser requestUser, List<string> sessionRoles);
    public AUser? DeleteUser(int id);
    public List<AUser> GetUsers(int page, int pageSize, Dictionary<string, object>? filters);
    public int GetAmountOfUsers(Dictionary<string, object>? filters);
    void ValidateCredentials(string email, string password);
    AUser GetByEmail(string email);
    AUser GetById(int userId);
    AUser UpdateUser(int userId, RequestUpdateUser request);
}