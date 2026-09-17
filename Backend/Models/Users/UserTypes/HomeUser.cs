using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ModelsAPI.Users;

namespace Models.Users.UserTypes;

public class HomeUser : AHomeUser
{

    public HomeUser()
    {
    }
    
    public HomeUser(AUser user) : base(user)
    {
    }

    public HomeUser(string firstName, string lastName, string email, string password, string profilePhoto) :base(new User(firstName, lastName, email, password),profilePhoto)
    {
    }
    
    public HomeUser(RequestAddUser requestAddUser) : this(requestAddUser.FirstName, requestAddUser.LastName, requestAddUser.Email, requestAddUser.Password, requestAddUser.ProfilePhoto)
    {
    }

}