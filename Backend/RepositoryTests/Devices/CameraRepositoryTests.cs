using Microsoft.EntityFrameworkCore;
using ModelInterface.Companys;
using ModelInterface.Devices;
using Models.Devices;
using Moq;
using Repositories.DeviceRepositories;

namespace RepositoryTests.Devices;

[TestClass]
public class CameraRepositoryTests
{
    private Mock<ACompany> _company;
    private Camera _camera;
    private Mock<DbContext> _dbContext;
    private IQueryable<Camera> _data;
    private Mock<DbSet<Camera>> _mockSet;
    private CameraRepository _cameraRepository;
    
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Mock<ACompany>();
        _camera = new Camera("Name", "1", "Description", "Photos", _company.Object, true, true, true) {Id = 1};
        _data = new List<Camera>
        {
            _camera
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<Camera>>();
        
        _mockSet.As<IQueryable<Camera>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Camera>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Camera>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Camera>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<Camera>()).Returns(_mockSet.Object);
        _cameraRepository = new CameraRepository(_dbContext.Object);
    }

    [TestMethod]
    public void CreateCamera_ShouldAddNewCamera()
    {
        _cameraRepository.Create(_camera);
        
        _mockSet.Verify(m => m.Add(It.IsAny<Camera>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());   
    }
    
    [TestMethod]
    public void GetById_ShouldReturnCamera()
    {
        var camera = _cameraRepository.GetById(1);
        
        Assert.AreEqual(_camera, camera);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllCameras()
    {
        var cameras = _cameraRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), cameras.Count);
    }

    [TestMethod]
    public void GetById_ShouldThrowException_WhenCameraIsNotFound()
    {
        Assert.IsNull(_cameraRepository.GetById(2));
    }
    
    
}