using System.Reflection;
using IRepositories.Repositories.HomesRepositories;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelsAPI.Homes;
using ModelInterface.Users.UserType;
using Moq;
using Services.Homes;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Users;

namespace ServicesTests.Homes;

[TestClass]
public class HomeServiceTest
{
    private Mock<IHomeRepository> _homeRepository;
    private Mock<IHomeUserService> _homeUserService;
    private Mock<AHomeUser> _user;
    private IHomeServices _homeServices;
    private Mock<AHome> _home;
    
    [TestInitialize]
    public void Setup()
    {
        _homeRepository = new Mock<IHomeRepository>();
        _homeUserService = new Mock<IHomeUserService>();
        _homeServices = new HomeService(_homeRepository.Object, _homeUserService.Object);
        _home = new Mock<AHome>();
        _user = new Mock<AHomeUser>();
        _user.Setup(x => x.UserId).Returns(1);
        _homeUserService.Setup(x => x.GetHomeUser(It.IsAny<int>())).Returns(_user.Object);
    }

    [TestMethod]
    public void AddHomeTest()
    {
        var request = new RequestCreateHome
        {
            Name = "Test Home",
            Address = new Address { MainStreet = "Main St", DoorNumber = 123 },
            Geolocation = new Geolocation { Latitude = 40.7128, Longitude = -74.0060 },
            MaxMembers = 5
        };
        
        _home = new Mock<AHome>();
        _home.Setup(x => x.Members).Returns(new List<AHomeMember>());
        _homeRepository.Setup(x => x.Create(It.IsAny<AHome>())).Returns(_home.Object);
        var result = _homeServices.AddHome(request, 1);
       
        _homeRepository.Verify(x => x.Create(It.Is<AHome>(h => h.Name == request.Name && h.Members.Count == 1)), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(request.Name, result.Name);
    }
    
    [TestMethod]
    public void GetHomeTest()
    {
        _home = new Mock<AHome>();
        _homeRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_home.Object);
        var result = _homeServices.GetHome(1);
        Assert.AreEqual(_home.Object, result);
    }
    
    [TestMethod]
    public void GetAllHomesTest()
    {
        List<AHome> homes = new List<AHome>();
        _homeRepository.Setup(x => x.GetAll()).Returns(homes);
        var result = _homeServices.GetAllHomes();
        Assert.AreEqual(homes, result);
    }
    
    [TestMethod]
    public void UpdateTest()
    {
        _home = new Mock<AHome>();
        _homeServices.Update(_home.Object);
        _homeRepository.Verify(x => x.Update(_home.Object), Times.Once);
    }
    
    [TestMethod]
    public void IsOwnerTest()
    {
        _home = new Mock<AHome>();
        _home.Setup(x => x.Owner).Returns(_user.Object);
        _home.Setup(x => x.HomeOwnerId).Returns(1);
        
        _homeRepository.Setup(x => x.GetById(It.Is<int>(id => id > 0))).Returns(_home.Object);
        _homeUserService.Setup(x => x.GetHomeUser(It.Is<int>(id => id > 0))).Returns(_user.Object);
        
        var res = _homeServices.IsOwner(1, 1);
        
        Assert.IsTrue(res);
    }
    
    [TestMethod]
    public void AddRoomToHomeTest()
    {
        _home = new Mock<AHome>();
        _home.Setup(x => x.Rooms).Returns(new List<ARoom>());
        _homeServices.AddRoomToHome(_home.Object, new Mock<ARoom>().Object);
        _homeRepository.Verify(x => x.Update(_home.Object), Times.Once);
        Assert.AreEqual(1, _home.Object.Rooms.Count);
    }
    
    [TestMethod]
    public void GetMyHomesTest()
    {
        List<AHome> homes = new List<AHome>();
        _homeRepository.Setup(x => x.GetMyHomes(It.IsAny<int>())).Returns(homes);
        var result = _homeServices.GetMyHomes(1);
        Assert.AreEqual(homes, result);
    }
}
    