using IRepositories.CRUD;
using ModelInterface.Devices;

namespace IRepositories.Repositories.DeviceRepositories;

public interface ICameraRepository : ICreateRepository<ACamera>, IGetRepository<ACamera>
{
    
}