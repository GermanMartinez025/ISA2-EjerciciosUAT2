using IRepositories.Repositories.DeviceRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using Models.Devices;

namespace Repositories.DeviceRepositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ADevice> _devices;
    private readonly DbSet<Camera> _cameras;
    private readonly DbSet<Sensor> _sensors;
    private readonly DbSet<MotionSensor> _motionSensors;
    private readonly DbSet<SmartLamp> _smartLamps;
    
    public DeviceRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _devices = dbContext.Set<ADevice>();
        _cameras = dbContext.Set<Camera>();
        _sensors = dbContext.Set<Sensor>();
        _motionSensors = dbContext.Set<MotionSensor>();
        _smartLamps = dbContext.Set<SmartLamp>();
    }
    
    public ADevice Create(ADevice device)
    {
        ADevice deviceExpected = device switch
        {
            Camera camera => _cameras.Add(camera).Entity,
            Sensor sensor => _sensors.Add(sensor).Entity,
            MotionSensor motionSensor => _motionSensors.Add(motionSensor).Entity,
            SmartLamp smartLamp => _smartLamps.Add(smartLamp).Entity,
        };
        
        _dbContext.SaveChanges();
        return deviceExpected;
    }
    


    public ADevice? GetById(int id)
    {
        return GetDevices().FirstOrDefault(d => d.Id == id);
    }

    public List<ADevice> GetAll()
    {
        return GetDevices().ToList();
    }

    public int Count(Dictionary<string, object>? filters)
    {
        IEnumerable<IDevice> devices = ApplyFilters( filters);
    
        return devices.Count();
    }

    public List<IDevice> GetPaginated(int page, int pageSize, Dictionary<string, object>? filters)
    {
        IEnumerable<IDevice> devices = _devices.OfType<ADevice>();
        devices = ApplyFilters(filters);
        
        return devices.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    private IEnumerable<IDevice> ApplyFilters( Dictionary<string, object>? filters)
    {
        if (filters == null || filters.Count == 0) return GetDevices();
        
        IEnumerable<IDevice> filterdDevices;
        
        if (filters.TryGetValue("deviceType", out var deviceType))
            filterdDevices = FilterByDeviceType((List<string>) deviceType);
        else filterdDevices = GetDevices();
        
        
        foreach (var filter in filters)
        {
            filterdDevices = filter.Key switch
            {
               "deviceName" => FilterByDeviceName(filterdDevices,(List<string>) filter.Value),
                "deviceModel" => FilterByDeviceModel(filterdDevices,(List<string>) filter.Value),
                "companyName" => FilterByCompanyName(filterdDevices,(List<string>) filter.Value),
                _ => filterdDevices 
            };
        }
        return filterdDevices;
    }

    private IEnumerable<IDevice> FilterByDeviceName(IEnumerable<IDevice> devices, List<string> deviceNames)
    {
        if (deviceNames.Count == 0) return devices;
        var query = devices.AsQueryable();
        return query.Include(d => d.Company)
            .Where(d => deviceNames.Contains(d.Name))
            .ToList();
    }
    
    private IEnumerable<IDevice> FilterByDeviceModel(IEnumerable<IDevice> devices, List<string> deviceModels)
    {
        if (deviceModels.Count == 0) return devices;
        var query = devices.AsQueryable();
        return query.Include(d => d.Company)
            .Where(d => deviceModels.Contains(d.ModelNumber))
            .ToList();
    }
    
    private IEnumerable<IDevice> FilterByCompanyName(IEnumerable<IDevice> devices, List<string> companyNames)
    {
        if (companyNames.Count == 0) return devices;
        var query = devices.AsQueryable();
        return query.Include(d => d.Company)
            .Where(d => companyNames.Contains(d.Company.Name))
            .ToList();
    }
    private IEnumerable<IDevice> FilterByDeviceType( List<string> deviceTypes)
    {
        if (deviceTypes.Count == 0) return GetDevices();
        IEnumerable<IDevice> filteredDevices = new List<IDevice>();
        foreach (var deviceType in deviceTypes)
        {
            filteredDevices = deviceType switch
            {
                "Camera" => filteredDevices.Concat(GetCameras()),
                "Sensor" => filteredDevices.Concat(GetSensors()),
                "MotionSensor" => filteredDevices.Concat(GetMotionSensors()),
                "SmartLamp" => filteredDevices.Concat(GetSmartLamps()),
                _ => filteredDevices
            };
        }
        
        return filteredDevices;
    }
    
    private IEnumerable<ADevice> GetDevices()
    {
        IEnumerable<ADevice> devices = new List<ADevice>();
        devices = devices.Concat(GetCameras());
        devices = devices.Concat(GetSensors());
        devices = devices.Concat(GetMotionSensors());
        devices = devices.Concat(GetSmartLamps());
        
        return devices;
    }
    
    private IEnumerable<Camera> GetCameras()
    {
        return _cameras
            .Include(c => c.Company)
            .Include(c => c.Photos);
    }
    
    private IEnumerable<Sensor> GetSensors()
    {
        return _sensors
            .Include(s => s.Company)
            .Include(s => s.Photos);
    }
    
    private IEnumerable<MotionSensor> GetMotionSensors()
    {
        return _motionSensors
            .Include(m => m.Company)
            .Include(m => m.Photos);
    }
    
    private IEnumerable<SmartLamp> GetSmartLamps()
    {
        return _smartLamps
            .Include(s => s.Company)
            .Include(s => s.Photos);
    }
    
}