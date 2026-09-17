using IRepositories.Repositories.DeviceRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using Models.Devices;

namespace Repositories.DeviceRepositories;

public class SensorRepository : ISensorRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Sensor> _sensors;

    public SensorRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _sensors = dbContext.Set<Sensor>();
    }
    
    public ASensor Create(ASensor sensor)
    {
        _sensors.Add((Sensor)sensor);
        _dbContext.SaveChanges();

        return sensor;
    }

    public ASensor? GetById(int id)
    {
        return _sensors.FirstOrDefault(s => s.Id == id);
    }

    public List<ASensor> GetAll()
    {
        return new List<ASensor>(_sensors.ToList());

    }
}