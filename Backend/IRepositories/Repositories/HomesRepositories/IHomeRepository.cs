using IRepositories.CRUD;
using ModelInterface.Homes;

namespace IRepositories.Repositories.HomesRepositories;

public interface IHomeRepository: ICreateRepository<AHome>, IGetRepository<AHome>, IUpdateRepository<AHome>
{
    public List<AHome> GetMyHomes(int userId);
}