using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using ModelsAPI.Users;

namespace Services.Users.FabricUsers;

public class ConcreteFabricAdmin : FabricUser
{
    public override AUserType CreateConcreteUser(RequestAddUser requestAddUser)
    {
        return new Admin( requestAddUser.FirstName, requestAddUser.LastName, requestAddUser.Email,requestAddUser.Password);
    }
}