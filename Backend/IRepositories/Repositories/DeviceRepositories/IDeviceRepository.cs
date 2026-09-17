using IRepositories.CRUD;
using ModelInterface.Devices;

namespace IRepositories.Repositories.DeviceRepositories;

public interface IDeviceRepository : IGetRepository<ADevice>, IGetPaginatedRepository<IDevice>, ICreateRepository<ADevice>
{
    public int Count(Dictionary<string, object>? filters);
    public ADevice GetById(int id);

    
}