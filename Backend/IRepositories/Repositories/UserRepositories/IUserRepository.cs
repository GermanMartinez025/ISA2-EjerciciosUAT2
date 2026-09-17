using IRepositories.CRUD;
using ModelInterface.Users;
using Models.Users;

namespace IRepositories.Repositories.UserRepositories;

public interface IUserRepository : IGetRepository<AUser>, IGetPaginatedRepository<AUser>, ICountRepository, IUpdateRepository<AUser>, IDeleteRepository<AUser>, ICreateRepository<AUser>
{
    public AUser? GetByEmail(string email);
    public AUser? ValidateCredentials(string email, string password);
}