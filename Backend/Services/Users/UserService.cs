using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Services.Users.FabricUsers;
using ServicesInterfaces;
using ServicesInterfaces.Users;

namespace Services.Users;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public AUser AddUser(RequestAddUser requestUser, List<string> sessionRoles)
    {
        ValidateRoles(sessionRoles, requestUser.Role);
        CheckAvailableEmail(requestUser.Email);

        var user = FabricUser.CreateUser(requestUser);

        _userRepository.Create(user.User);

        return user.User;
    }

    public AUser? DeleteUser(int id)
    {
        AUser user = _userRepository.GetById(id);

        ValidateUserExists(user);

        ValidateUserIsAdmin(user, id);

        _userRepository.Delete(user);
        return user;
    }

    public List<AUser> GetUsers(int page, int pageSize, Dictionary<string, object>? filters)
    {
        return _userRepository.GetPaginated(page, pageSize, filters);
    }

    public int GetAmountOfUsers(Dictionary<string, object>? filters)
    {
        return filters == null ? _userRepository.Count() : _userRepository.Count(filters);
    }

    public void ValidateCredentials(string email, string password)
    {
        AUser? user = _userRepository.ValidateCredentials(email, password);

        if (user == null)
        {
            throw new NotFoundException("Invalid credentials, please check.");
        }
    }

    public AUser GetByEmail(string email)
    {
        AUser user = _userRepository.GetByEmail(email);

        ValidateUserExists(user);

        return user;
    }

    public AUser GetById(int userId)
    {
        AUser user = _userRepository.GetById(userId);

        ValidateUserExists(user);

        return user;
    }

    public AUser UpdateUser(int userId, RequestUpdateUser request)
    {
        AUser user = null;
        user = _userRepository.GetById(userId);
        
        ValidateUserExists(user);
        
        int updated = 0;
        foreach (var prop in request.GetType().GetProperties())
        {
            var value = prop.GetValue(request);
            if (value == null) continue;
            user.Update(prop.Name, value);
            updated++;
        }

        if (updated == 0)
        {
            throw new ConflictException("No fields to update");
        }

        _userRepository.Update(user);

        return user;
    }

    private void CheckAvailableEmail(string email)
    {
        AUser user = null;
        user = _userRepository.GetByEmail(email);
     
        if (user != null)
        {
            throw new ConflictException("Email already in use");
        }
    }

    private void ValidateRoles(List<string> sessionRoles, string requestRole)
    {
        bool isAllowed = sessionRoles.Any(sessionRole => ValidateRole(sessionRole, requestRole));

        if (!isAllowed)
        {
            throw new ConflictException($"You can't add a user with role '{requestRole}'");
        }
    }

    private bool ValidateRole(string sessionRole, string requestRole)
    {
        bool result = false;
        switch (sessionRole)
        {
            case "Admin":
                if (requestRole is "Admin" or "CompanyOwner") result = true;
                break;
            case "NoSession":
                if (requestRole is "HomeUser") result = true;
                break;
        }

        return result;
    }

    private void ValidateUserExists(AUser user)
    {
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
    }

    private void ValidateUserIsAdmin(AUser user, int id)
    {
        if (!user.IsAdmin())
        {
            throw new ConflictException($"The user with the id {id} isn't an admin");
        }
    }
    
}