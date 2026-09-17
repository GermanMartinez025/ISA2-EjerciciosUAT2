using IRepositories.CRUD;
using ModelInterface.Devices;

namespace IRepositories.Repositories.DeviceRepositories;

public interface ISensorRepository : ICreateRepository<ASensor>, IGetRepository<ASensor>
{
}