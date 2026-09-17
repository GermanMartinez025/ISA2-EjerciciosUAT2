using IRepositories.CRUD;
using ModelInterface.Sessions;

namespace IRepositories.Repositories.SessionRepositories;

public interface ISessionRepository : ICreateRepository<ISession>, IDeleteRepository<ISession>, IGetRepository<ISession>
{
    ISession GetByToken(string token);
}