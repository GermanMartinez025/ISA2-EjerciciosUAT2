using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;
using Moq;
using Repositories.UserRepositories;

namespace RepositoryTests.Users;

[TestClass]
public class AdminRepositoryTests
{
    private Mock<Admin> _admin;
    private IQueryable<Admin> _data;
    private Mock<DbSet<Admin>> _mockSet;
    private Mock<DbContext> _dbContext;
    private AdminRepository _adminRepo;

    [TestInitialize]
    public void Setup()
    {
        _admin = new Mock<Admin>();
        _admin.Setup(a => a.UserId).Returns(1);
        _data = new List<Admin> { _admin.Object }.AsQueryable();

        _mockSet = new Mock<DbSet<Admin>>();

        _mockSet.As<IQueryable<AAdmin>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<AAdmin>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<AAdmin>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<AAdmin>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();

        _dbContext.Setup(c => c.Set<Admin>()).Returns(_mockSet.Object);

        _adminRepo = new AdminRepository(_dbContext.Object);
    }
    
    [TestMethod]
    public void GetById_ShouldReturnAdmin()
    {
        var result = _adminRepo.GetById(1);
        
        Assert.AreEqual(_admin.Object, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllAdmins()
    {
        var result = _adminRepo.GetAll();
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
    
    [TestMethod]
    public void Delete_ShouldDeleteAdmin()
    {
        _adminRepo.Delete(_admin.Object);
        
        _mockSet.Verify(m => m.Remove(It.IsAny<Admin>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
}