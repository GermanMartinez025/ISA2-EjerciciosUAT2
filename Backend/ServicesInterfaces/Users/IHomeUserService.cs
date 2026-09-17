using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ServicesInterfaces.Users;

public interface IHomeUserService
{
    public AHomeUser GetHomeUser(int id);
    public List<AHomeUser> GetAllHomeUsers();
    public AHomeUser GetHomeUserByEmail(string email);
    public void Update(AHomeUser user);
}