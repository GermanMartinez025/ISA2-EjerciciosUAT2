using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using ModelInterface.Homes;
using Models.Devices;
using Moq;
using Repositories.DeviceRepositories;

namespace RepositoryTests.Devices;

[TestClass]
public class HomeDeviceRepositoryTests
{
    private HomeDevice _homeDevice;
    private Mock<AHome> _home;
    private Mock<ADevice> _device;
    private Mock<DbContext> _dbContext;
    private IQueryable<HomeDevice> _data;
    private Mock<DbSet<HomeDevice>> _mockSet;
    private HomeDeviceRepository _homeDeviceRepository;
    private Guid guid = Guid.NewGuid();
    
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Mock<AHome>();
        _device = new Mock<ADevice>();
        
        _homeDevice = new HomeDevice(_device.Object, _home.Object){Online = true, HardwareId = guid};
        
        _data = new List<HomeDevice>
        {
            _homeDevice
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<HomeDevice>>();
        
        _mockSet.As<IQueryable<HomeDevice>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<HomeDevice>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<HomeDevice>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<HomeDevice>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<HomeDevice>()).Returns(_mockSet.Object);

        _homeDeviceRepository = new HomeDeviceRepository(_dbContext.Object);

    }

    [TestMethod]
    public void CreateHomeDevice_ShouldAddNewHomeDevice()
    {
        _homeDeviceRepository.Create(_homeDevice);
        
        _mockSet.Verify(m => m.Add(It.IsAny<HomeDevice>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());   
    }
    
    [TestMethod]
    public void GetById_ShouldReturnHomeDevice()
    {
        var homeDevice = _homeDeviceRepository.GetById(guid);
        
        Assert.AreEqual(_homeDevice, homeDevice);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllHomeDevices()
    {
        var homeDevices = _homeDeviceRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), homeDevices.Count);
    }
    
    [TestMethod]
    public void DeleteHomeDevice_ShouldDeleteHomeDevice()
    {
        _homeDeviceRepository.Delete(_homeDevice);
        
        _mockSet.Verify(m => m.Remove(It.IsAny<HomeDevice>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void SaveChanges_ShouldCalled_SaveChanges()
    {
        _homeDeviceRepository.SaveChanges();
        
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
}