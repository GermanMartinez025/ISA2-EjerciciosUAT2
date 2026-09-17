using IRepositories.CRUD;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace IRepositories.Repositories.UserRepositories;

public interface ICompanyOwnerRepository :  IGetRepository<ACompanyOwner>, IUpdateRepository<ACompanyOwner>
{
    
}