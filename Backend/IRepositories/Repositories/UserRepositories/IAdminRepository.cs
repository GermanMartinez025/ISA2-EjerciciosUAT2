using IRepositories.CRUD;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace IRepositories.Repositories.UserRepositories;

public interface IAdminRepository : IDeleteRepository<AAdmin>, IGetRepository<AAdmin>
{
    
}