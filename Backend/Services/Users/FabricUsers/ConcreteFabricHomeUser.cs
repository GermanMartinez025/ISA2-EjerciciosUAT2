using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using ModelsAPI.Users;

namespace Services.Users.FabricUsers;

public class ConcreteFabricHomeUser : FabricUser
{
    public override AUserType CreateConcreteUser(RequestAddUser requestAddUser)
    {
        return new HomeUser(requestAddUser.FirstName, requestAddUser.LastName, requestAddUser.Email, requestAddUser.Password,requestAddUser.ProfilePhoto);
    }
}