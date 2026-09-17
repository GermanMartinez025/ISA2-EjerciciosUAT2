using IRepositories.CRUD;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace IRepositories.Repositories.UserRepositories;

public interface IHomeUserRepository : IGetRepository<AHomeUser>, IUpdateRepository<AHomeUser>
{
    public AHomeUser GetByEmail(string email);
    public AHomeUser Update(AHomeUser user);
}