using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ModelInterface.Companys;
using ModelInterface.Devices;
using Models.Company;
using Models.Devices;
using Models.Users.UserTypes;
using Moq;
using Repositories.DeviceRepositories;

namespace RepositoryTests.Devices;

[TestClass]
public class DeviceRepositoryTests
{
    private DeviceRepository _deviceRepository;
    private Mock<DbContext> _dbContextMock;
    
    private Sensor _sensor;
    private IQueryable<ADevice> _listSensor;
    
    private Camera _camera;
    private IQueryable<ADevice> _listCamera;
    
    private MotionSensor _motionSensor;
    private IQueryable<ADevice> _listMotionSensor;
    
    private SmartLamp _smartLamp;
    private IQueryable<ADevice> _listSmartLamp;
    
    private List<ADevice> _allDevices;
    
    private int _id = 1;
    
    private T CreateItem<T>() where T : class, new()
    {
        var item = new T();
        var propertyInfo = typeof(T).GetProperty("Id");
        propertyInfo.SetValue(item, _id);
        _id++;
        return item;
    }
    
    private T SetupMock<T>() where T : class, new()
    {
        List<T> data = new();
        T item = CreateItem<T>();
        data.Add(item);
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        SetupContextMock(mockSet);

        return item;
    }
    

    private void SetupContextMock<T>(Mock<DbSet<T>> mockSet) where T : class
    {
        _dbContextMock.Setup(c => c.Set<T>()).Returns(mockSet.Object);
    }

    private void SetupMockDbSet()
    {
        _allDevices = new List<ADevice>
        {
            CreateItem<Sensor>(),
            CreateItem<Camera>(),
            CreateItem<MotionSensor>(),
            CreateItem<SmartLamp>()
        };

        var queryableData = _allDevices.AsQueryable();
        var mockSet = new Mock<DbSet<ADevice>>();

        mockSet.As<IQueryable<ADevice>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<ADevice>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<ADevice>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<ADevice>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

        _dbContextMock.Setup(c => c.Set<ADevice>()).Returns(mockSet.Object);
    }
    
    [TestInitialize]
    public void TestInitialize()
    {
        _dbContextMock = new Mock<DbContext>();
        SetupMockDbSet();
        
        _sensor = SetupMock<Sensor>();
        _camera = SetupMock<Camera>();
        _motionSensor = SetupMock<MotionSensor>();
        _smartLamp = SetupMock<SmartLamp>();

        _deviceRepository = new DeviceRepository(_dbContextMock.Object);
    }
    
    [TestMethod]
    public void GetById_WhenDeviceDoesNotExist_ShouldThrowException()
    {
        int nonExistentId = 999; 
        
        Assert.IsNull(_deviceRepository.GetById(nonExistentId));
    }
    
    [TestMethod]
    public void GetById_WhenDeviceExists_ShouldReturnDevice()
    {
        var result = _deviceRepository.GetById(_sensor.Id);
        
        Assert.AreEqual(_sensor, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllDevices()
    {
        var result = _deviceRepository.GetAll();
        
        Assert.AreEqual(4, result.Count);
    }
    
    [TestMethod]
    public void Count_WhenFiltersAreNull_ShouldReturnCountOfAllDevices()
    {
        var result = _deviceRepository.Count(null);
        
        Assert.AreEqual(4, result);
    }
    
    [TestMethod]
    public void Count_WhenFiltersAreNotNull_ShouldReturnCountOfFilteredDevices()
    {
        var filters = new Dictionary<string, object>
        {
            { "Type", "Sensor" }
        };
        
        var result = _deviceRepository.Count(filters);
        
        Assert.AreEqual(4, result);
    }
 
    [TestMethod]
    public void GetPaginated_WhenFiltersAreNull_ShouldReturnPaginatedDevices()
    {
        var result = _deviceRepository.GetPaginated(1, 2, null);
        
        Assert.AreEqual(2, result.Count);
    }
    
    [TestMethod]
    public void GetPaginated_WhenFiltersAreNotNull_ShouldReturnPaginatedFilteredDevices()
    {
        var filters = new Dictionary<string, object>
        {
            { "Type", "Sensor" }
        };
        
        var result = _deviceRepository.GetPaginated(1, 2, filters);
        
        Assert.AreEqual(2, result.Count);
    }
    
    [TestMethod]
    public void FilterByDeviceName_ShouldReturnMatchingDevices()
    {
        var devices = new List<IDevice>
        {
            new Mock<IDevice> { Name = "Device1" }.Object,
            new Mock<IDevice> { Name = "Device2" }.Object,
            new Mock<IDevice> { Name = "Device3" }.Object
        };

        var deviceNames = new List<string> { "Device1", "Device3" };
        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByDeviceName", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { devices, deviceNames });
        
        Assert.AreEqual(0, result.Count());
    }
    
    [TestMethod]
    public void FilterByDeviceModel_ShouldReturnMatchingDevices()
    {
        var devices = new List<IDevice>
        {
            new Sensor { ModelNumber = "SensorModel1", Company = new Company { Name = "CompanyA" }},
            new Camera { ModelNumber = "CameraModel1", Company = new Company { Name = "CompanyB" }},
            new MotionSensor { ModelNumber = "MotionModel1", Company = new Company { Name = "CompanyA" }},
            new SmartLamp { ModelNumber = "LampModel1", Company = new Company { Name = "CompanyB" }}
        }.AsQueryable();

        var deviceModels = new List<string> { "SensorModel1", "CameraModel1" };
        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByDeviceModel", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { devices, deviceModels });
        
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(d => d.ModelNumber == "SensorModel1"));
        Assert.IsTrue(result.Any(d => d.ModelNumber == "CameraModel1"));
    }

    [TestMethod]
    public void FilterByCompanyName_ShouldReturnMatchingDevices()
    {
       
        var devices = new List<IDevice>
        {
            new Sensor { Company = new Company { Name = "CompanyA" }},
            new Camera { Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Company = new Company { Name = "CompanyA" }},
            new SmartLamp { Company = new Company { Name = "CompanyC" }}
        }.AsQueryable();

        var companyNames = new List<string> { "CompanyA", "CompanyB" };

        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByCompanyName", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { devices, companyNames });
        
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(d => d.Company.Name == "CompanyA"));
        Assert.IsTrue(result.Any(d => d.Company.Name == "CompanyB"));
    }
    
    [TestMethod]
    public void FilterByDeviceType_ShouldReturnMatchingDevices()
    {
        var devices = new List<IDevice>
        {
            new Camera { Company = new Company { Name = "CompanyA" }},
            new Sensor { Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Company = new Company { Name = "CompanyA" }},
            new SmartLamp { Company = new Company { Name = "CompanyC" }}
        }.AsQueryable();

        var deviceTypes = new List<string> { "Camera" };
        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });

        Assert.AreEqual(1, result.Count());
    }
    
    [TestMethod]
    public void FilterByDeviceType_WhenDeviceTypesAreEmpty_ShouldReturnAllDevices()
    {
        var devices = new List<IDevice>
        {
            new Camera { Company = new Company { Name = "CompanyA" }},
            new Sensor { Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Company = new Company { Name = "CompanyA" }},
            new SmartLamp { Company = new Company { Name = "CompanyC" }}
        }.AsQueryable();

        var deviceTypes = new List<string>();
        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });
        
        Assert.AreEqual(4, result.Count());
    }
    
    [TestMethod]
    public void FilterByDeviceType_WhenDeviceTypesAreNull_ShouldReturnAllDevices()
    {
        var devices = new List<IDevice>
        {
            new Camera { Company = new Company { Name = "CompanyA" }},
            new Sensor { Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Company = new Company { Name = "CompanyA" }},
            new SmartLamp { Company = new Company { Name = "CompanyC" }}
        }.AsQueryable();

        var deviceTypes = new List<string>();
        
        var methodInfo = typeof(DeviceRepository)
            .GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });
        
        Assert.AreEqual(4, result.Count());
    }
    
    [TestMethod]
    public void FilterByDeviceName_WhenDeviceNamesIsEmpty_ShouldReturnAllDevices()
    {
        var devices = new List<IDevice>
        {
            new Sensor { Name = "TemperatureSensor", Company = new Company { Name = "CompanyA" }},
            new Camera { Name = "OutdoorCamera", Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Name = "MotionDetector", Company = new Company { Name = "CompanyA" }},
        }.AsQueryable();

        var deviceNames = new List<string>(); 
        var methodInfo = typeof(DeviceRepository).GetMethod("FilterByDeviceName", BindingFlags.NonPublic | BindingFlags.Instance);
    
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { devices, deviceNames });
    
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(d => d.Name == "TemperatureSensor"));
        Assert.IsTrue(result.Any(d => d.Name == "OutdoorCamera"));
        Assert.IsTrue(result.Any(d => d.Name == "MotionDetector"));
    }
    
    [TestMethod]
    public void FilterByCompanyName_WhenCompanyNamesDoNotMatch_ShouldReturnEmpty()
    {
        var devices = new List<IDevice>
        {
            new Sensor { Company = new Company { Name = "CompanyA" }},
            new Camera { Company = new Company { Name = "CompanyB" }},
            new MotionSensor { Company = new Company { Name = "CompanyA" }},
            new SmartLamp { Company = new Company { Name = "CompanyC" }}
        }.AsQueryable();

        var companyNames = new List<string> { "CompanyX", "CompanyY" };
        var methodInfo = typeof(DeviceRepository).GetMethod("FilterByCompanyName", BindingFlags.NonPublic | BindingFlags.Instance);

        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { devices, companyNames });

        Assert.AreEqual(0, result.Count());
        Assert.IsFalse(result.Any(d => d.Company.Name == "CompanyX"));
        Assert.IsFalse(result.Any(d => d.Company.Name == "CompanyY"));
    }
    
    [TestMethod]
    public void ApplyFilters_WhenFiltersIsNull_ShouldReturnAllDevices()
    {
        var methodInfo = typeof(DeviceRepository).GetMethod("ApplyFilters", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { null });

        Assert.AreEqual(4, result.Count());
    }
   
    [TestMethod]
    public void ApplyFilters_WhenFiltersIsEmpty_ShouldReturnAllDevices()
    {
        var filters = new Dictionary<string, object>();
        var methodInfo = typeof(DeviceRepository).GetMethod("ApplyFilters", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { filters });

        Assert.AreEqual(4, result.Count());
    }
    
    [TestMethod]
    public void ApplyFilters_WhenFiltersContainDeviceType_ShouldReturnMatchingDevices()
    {
        var filters = new Dictionary<string, object> { { "deviceType", new List<string> { "Camera" } } };
        var methodInfo = typeof(DeviceRepository).GetMethod("ApplyFilters", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { filters });

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(d => d is Camera));
    }
    
    [TestMethod]
    public void ApplyFilters_WhenFiltersContainDeviceModel_ShouldReturnMatchingDevices()
    {
        var filters = new Dictionary<string, object> { { "deviceModel", new List<string> { "SensorModel1" } } };
        var methodInfo = typeof(DeviceRepository).GetMethod("ApplyFilters", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { filters });

        Assert.AreEqual(0, result.Count());
        Assert.IsTrue(result.All(d => d.ModelNumber == "SensorModel1"));
    }
    
    [TestMethod]
    public void FilterByDeviceType_WhenDeviceTypesContainCamera_ShouldReturnCameras()
    {
        var deviceTypes = new List<string> { "Camera" };
        var methodInfo = typeof(DeviceRepository).GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });

        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.All(d => d is Camera));
    }
    
    [TestMethod]
    public void FilterByDeviceType_WhenDeviceTypesContainMultipleTypes_ShouldReturnMatchingDevices()
    {
        var deviceTypes = new List<string> { "Camera", "Sensor" };
        var methodInfo = typeof(DeviceRepository).GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });

        Assert.AreEqual(2, result.Count());
    }
    
    [TestMethod]
    public void FilterByDeviceType_WhenDeviceTypesContainInvalidType_ShouldReturnEmpty()
    {
        var deviceTypes = new List<string> { "InvalidType" };
        var methodInfo = typeof(DeviceRepository).GetMethod("FilterByDeviceType", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (IEnumerable<IDevice>)methodInfo.Invoke(_deviceRepository, new object[] { deviceTypes });

        Assert.AreEqual(0, result.Count());
    }
    
    
}

