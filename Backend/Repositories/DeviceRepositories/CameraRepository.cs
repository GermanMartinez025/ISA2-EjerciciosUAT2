using IRepositories.Repositories.DeviceRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using Models.Devices;

namespace Repositories.DeviceRepositories;

public class CameraRepository : ICameraRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Camera> _cameras;
    
    public CameraRepository (DbContext dbContext)
    {
        _dbContext = dbContext;
        _cameras = dbContext.Set<Camera>();
    }

    public ACamera Create(ACamera device)
    {
        _cameras.Add((Camera)device);
        _dbContext.SaveChanges();

        return (Camera)device;
    }

    public ACamera? GetById(int id)
    {
        return _cameras.FirstOrDefault(c => c.Id == id);
    }

    public List<ACamera> GetAll()
    {
        return new List<ACamera>(_cameras.ToList());
    }
    
}