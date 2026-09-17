using IRepositories.Repositories.DeviceRepositories;
using Microsoft.EntityFrameworkCore;
using Models.Devices;

namespace Repositories.DeviceRepositories;

public class HomeDeviceRepository : IHomeDeviceRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<HomeDevice> _homeDevices;
    
    public HomeDeviceRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _homeDevices = dbContext.Set<HomeDevice>();
    }
    
    public HomeDevice Create(HomeDevice entity)
    {
        _homeDevices.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public HomeDevice? GetById(Guid id)
    {
        var homeDevice = _homeDevices
            .Include(h=>h.Device)
            .Include(h=>h.Home)
            .ThenInclude(h=>h.Members)
            .FirstOrDefault(hd => hd.HardwareId == id);
        
        return homeDevice;
    }

    public List<HomeDevice> GetAll()
    {
        return _homeDevices.ToList();
    }

    public void Delete(HomeDevice homeDevice)
    {
        _homeDevices.Remove(homeDevice);
        _dbContext.SaveChanges();
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}