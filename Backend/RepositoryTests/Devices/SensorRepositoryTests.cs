using Microsoft.EntityFrameworkCore;
using ModelInterface.Companys;
using ModelInterface.Devices;
using Models.Devices;
using Moq;
using Repositories.DeviceRepositories;

namespace RepositoryTests.Devices;

[TestClass]
public class SensorRepositoryTests
{
    private Mock<ACompany> _company;
    private Sensor _sensor;
    private Mock<DbContext> _dbContext;
    private IQueryable<Sensor> _data;
    private Mock<DbSet<Sensor>> _mockSet;
    private SensorRepository _sensorRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Mock<ACompany>();
        _sensor = new Sensor("Name", "1", "Desc", "Photos", _company.Object) {Id = 1};
    
        _data = new List<Sensor>
        {
            _sensor
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<Sensor>>();
        
        _mockSet.As<IQueryable<Sensor>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Sensor>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Sensor>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Sensor>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<Sensor>()).Returns(_mockSet.Object);
        _sensorRepository = new SensorRepository(_dbContext.Object);
    }
    
    [TestMethod]
    public void CreateSensor_ShouldAddNewSensor()
    {
        _sensorRepository.Create(_sensor);
        
        _mockSet.Verify(m => m.Add(It.IsAny<Sensor>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void GetById_ShouldReturnSensor()
    {
        var sensor = _sensorRepository.GetById(1);
        
        Assert.AreEqual(_sensor, sensor);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllSensors()
    {
        var sensors = _sensorRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), sensors.Count);
    }
    
    [TestMethod]
    public void GetById_ShouldThrowException_WhenSensorDoesNotExist()
    {
        Assert.IsNull(_sensorRepository.GetById(2));
    }
    
}