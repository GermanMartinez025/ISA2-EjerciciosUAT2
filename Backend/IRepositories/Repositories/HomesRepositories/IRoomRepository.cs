using IRepositories.CRUD;
using ModelInterface.Homes;

namespace IRepositories.Repositories.HomesRepositories;

public interface IRoomRepository : ICreateRepository<ARoom>, IGetRepository<ARoom>, IDeleteRepository<ARoom>
{
    
}