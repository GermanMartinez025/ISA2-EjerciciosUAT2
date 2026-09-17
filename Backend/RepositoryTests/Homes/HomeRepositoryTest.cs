using Microsoft.EntityFrameworkCore;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Homes;
using Models.Users.UserTypes;
using Moq;
using Repositories.Homes;
using Repositories.UserRepositories;

namespace RepositoryTests.Homes;

[TestClass]
public class HomeRepositoryTest
{
    private AHome _home;
    private IQueryable<AHome> _data;
    private Mock<DbSet<AHome>> _mockSet;
    private Mock<DbContext> _dbContext;
    private HomeRepository _homeRepository;
    private Mock<AHomeUser> _homeUser;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeUser = new Mock<AHomeUser>();
        _homeUser.Setup(h => h.UserId).Returns(1);
        _home = new Home("Main Street", 123, "casita", 82.123, -123.123, 3, _homeUser.Object) { Id = 1 };
        
        _data = new List<AHome>
        {
            _home
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<AHome>>();
        
        _mockSet.As<IQueryable<AHome>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<AHome>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<AHome>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<AHome>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<AHome>()).Returns(_mockSet.Object);
        _homeRepository = new HomeRepository(_dbContext.Object);
        
    }
    
    [TestMethod]
    public void CreateHome_ShouldAddHome()
    {
        _homeRepository.Create(_home);
        
        _mockSet.Verify(m => m.Add(It.IsAny<AHome>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void GetById_ShouldReturnHomes()
    {
        var result = _homeRepository.GetById(1);
        
        Assert.AreEqual(_home, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllHomes()
    {
        var result = _homeRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
    
    [TestMethod]
    public void GetById_ShouldThrowException_WhenHomeNotFound()
    {
        Assert.IsNull(_homeRepository.GetById(2));
    }
    
    [TestMethod]
    public void GetById_ShouldThrowException_WhenIdLessThaZero()
    {
        Assert.IsNull(_homeRepository.GetById(-2));
    }

    [TestMethod]
    public void Update_ShouldUpdateHome()
    {
        _homeRepository.Update(_home);
        
        _mockSet.Verify(m => m.Update(It.IsAny<AHome>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void GetMyHomes_ShouldReturnMyHomes()
    {
        var result = _homeRepository.GetMyHomes(1);
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
}