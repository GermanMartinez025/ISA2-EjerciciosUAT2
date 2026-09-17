using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using Models.Users.UserTypes;
using Moq;
using Repositories.UserRepositories;

namespace RepositoryTests.Users;

[TestClass]
public class HomeUserRepositoryTests
{
    private Mock<HomeUser> _homeUser;
    private IQueryable<HomeUser> _data;
    private Mock<DbSet<HomeUser>> _mockSet;
    private Mock<DbContext> _dbContext;
    private HomeUserRepository _homeHomeUserRepo;
    
    [TestInitialize]
    public void Setup()
    {
        _homeUser = new Mock<HomeUser>();
        _homeUser.Setup(x => x.User).Returns(new Mock<AUser>().Object);
        _homeUser.Setup(hm => hm.UserId).Returns(1);
        _data = new List<HomeUser> { _homeUser.Object }.AsQueryable();
        
        _mockSet = new Mock<DbSet<HomeUser>>();
        
        _mockSet.As<IQueryable<HomeUser>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<HomeUser>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<HomeUser>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<HomeUser>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<HomeUser>()).Returns(_mockSet.Object);
        _homeHomeUserRepo = new HomeUserRepository(_dbContext.Object);
    }

    [TestMethod]
    public void GetById_ShouldReturnUserWithPhoto()
    {
        var result = _homeHomeUserRepo.GetById(1);
        
        Assert.AreEqual(_homeUser.Object, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnUserWithPhotos()
    {
        var result = _homeHomeUserRepo.GetAll();
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
    
    [TestMethod]
    public void GetById_ShouldThrowException_WhenUserWithPhotoNotFound()
    {
        Assert.IsNull(_homeHomeUserRepo.GetById(2));
    }

    [TestMethod]
    public void GetByEmail_ShouldReturnUserWithPhoto()
    {
        _homeUser.Setup(hm => hm.User.Email).Returns("john@example.com");

        var result = _homeHomeUserRepo.GetByEmail("john@example.com");
        
        Assert.AreEqual(_homeUser.Object, result);
    }

    [TestMethod]
    public void GetByEmail_ShouldThrowException_WhenUserWithPhotoNotFound()
    {
        Assert.IsNull(_homeHomeUserRepo.GetByEmail("noexiste@example.ort"));
    }

    [TestMethod]
    public void Update_ShouldUpdateHomeMember_WhenCalled()
    {
        _homeHomeUserRepo.Update(_homeUser.Object);
        
        _mockSet.Verify(m => m.Update(It.IsAny<HomeUser>()), Times.Once());
    }
    
    [TestMethod]
    public void Update_ShouldThrowException_WhenHomeMemberNotFound()
    {
        Assert.IsNull(_homeHomeUserRepo.Update(null));
    }
}