using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using ModelsAPI.Users;

namespace Services.Users.FabricUsers;

public class ConcreteFabirCompanyOwner : FabricUser
{
    public override AUserType CreateConcreteUser(RequestAddUser requestAddUser)
    {
        return new CompanyOwner(requestAddUser.FirstName, requestAddUser.LastName, requestAddUser.Email, requestAddUser.Password);
    }
}