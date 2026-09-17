using IRepositories.CRUD;
using Models.Devices;

namespace IRepositories.Repositories.DeviceRepositories;

public interface IHomeDeviceRepository : ICreateRepository<HomeDevice>, 
    IGetRepository<HomeDevice, Guid>, IDeleteRepository<HomeDevice> 
{
    void SaveChanges();
}