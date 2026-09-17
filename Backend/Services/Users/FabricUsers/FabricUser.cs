using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ModelsAPI.Users;

namespace Services.Users.FabricUsers;

public abstract class FabricUser
{
    public abstract AUserType CreateConcreteUser(RequestAddUser requestAddUser);
    
    public static FabricUser GetFabricUser(string role)
    {
        return role switch
        {
            "HomeUser" => new ConcreteFabricHomeUser(),
            "Admin" => new ConcreteFabricAdmin(),
            "CompanyOwner" => new ConcreteFabirCompanyOwner(),
            _ => throw new BadRequestException("Invalid Role")
        };
    }

    public static AUserType CreateUser(RequestAddUser requestAddUser)
    {
        var fabricUser = GetFabricUser(requestAddUser.Role);
        return fabricUser.CreateConcreteUser(requestAddUser);
    }

}