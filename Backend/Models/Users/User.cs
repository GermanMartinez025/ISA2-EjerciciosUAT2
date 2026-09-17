using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace Models.Users;

public class User : AUser
{

    public User()
    {
    }

    public User(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password)
    {
       
    }

    public override void AddRole(RolesEnum role)
    {
        AUserType userType = role switch
        {
            RolesEnum.Admin => new Admin(this),
            RolesEnum.CompanyOwner => new CompanyOwner(this),
            RolesEnum.HomeUser => new HomeUser(this),
        };
        
    }
}