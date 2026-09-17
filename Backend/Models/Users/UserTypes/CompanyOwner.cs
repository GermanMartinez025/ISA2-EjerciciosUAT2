using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace Models.Users.UserTypes;

public class CompanyOwner : ACompanyOwner
{

    public CompanyOwner()
    {
    }
    
    public CompanyOwner(AUser user) : base(user)
    {
    }

    public CompanyOwner(string firstName, string lastName, string email, string password) : base(new User(firstName, lastName,
        email, password))
    {
    }
}