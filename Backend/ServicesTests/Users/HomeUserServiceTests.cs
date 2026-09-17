using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Moq;
using Services.Users;
using ServicesInterfaces.Users;

namespace ServicesTests.Users;

[TestClass]
public class HomeUserServiceTests
{
    
    private Mock<IHomeUserRepository> _userWithPhotoRepository;
    private IHomeUserService _homeUserService;
    
    [TestInitialize]
    public void Setup()
    {
        _userWithPhotoRepository = new Mock<IHomeUserRepository>();
        _homeUserService = new HomeUserService(_userWithPhotoRepository.Object);
    }
    
    [TestMethod]
    public void GetHomeUserTest()
    {
        var homeUser = new Mock<AHomeUser>();
        _userWithPhotoRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(homeUser.Object);
        var result = _homeUserService.GetHomeUser(1);
        Assert.AreEqual(homeUser.Object, result);
    }
    
    [TestMethod]
    public void GetAllHomeUsersTest()
    {
        var homeUsers = new List<AHomeUser>();
        _userWithPhotoRepository.Setup(x => x.GetAll()).Returns(homeUsers);
        var result = _homeUserService.GetAllHomeUsers();
        Assert.AreEqual(homeUsers, result);
    }
    
    [TestMethod]
    public void GetHomeUserByEmailTest()
    {
        var homeUser = new Mock<AHomeUser>();
        _userWithPhotoRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(homeUser.Object);
        var result = _homeUserService.GetHomeUserByEmail("email@email.com");
        Assert.AreEqual(homeUser.Object, result);
    }
    
    [TestMethod]
    public void UpdateTest()
    {
        var homeUser = new Mock<AHomeUser>();
        _homeUserService.Update(homeUser.Object);
        _userWithPhotoRepository.Verify(x => x.Update(homeUser.Object), Times.Once);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void Update_ThrowsArgumentNullException_WhenHomeUserIsNull()
    {
        _homeUserService.Update(null);
    }
    
}