using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ServicesInterfaces.Users;

public interface IAdminService
{
    public AAdmin? DeleteAdmin(int adminId);
}