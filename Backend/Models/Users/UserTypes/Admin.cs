using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace Models.Users.UserTypes;

public class Admin : AAdmin
{
    public Admin()
    {
    }
    
    public Admin(AUser user) : base(user)
    {
    }
    
    public Admin(string firstName, string lastName, string email, string password) : base(new User(firstName, lastName,
        email, password))
    {
    }

}