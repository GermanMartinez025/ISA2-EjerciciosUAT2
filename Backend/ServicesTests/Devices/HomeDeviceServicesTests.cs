    using IRepositories.Repositories.DeviceRepositories;
    using ModelException;
    using ModelInterface.Devices;
    using ModelInterface.Homes;
    using ModelInterface.Notifications;
    using ModelInterface.Users;
    using Models.Devices;
    using Models.Users.UserTypes;
    using Moq;
    using Services.Devices;
    using ServicesInterfaces.Devices;
    using ServicesInterfaces.Homes;
    using ServicesInterfaces.Notifications;
    using ServicesInterfaces.Users;

    namespace ServicesTests.Devices;

[TestClass]
public class HomeDeviceServicesTests
{
    private Mock<IHomeDeviceRepository> _homeDeviceRepository;

    private Mock<IHomeServices> _homeServices;
    private Mock<IDeviceServices> _deviceServices;
    private HomeDeviceServices _homeDeviceServices;
    private Mock<IHomeMemberService> _homeMemberService;
    private Mock<IRoomServices> _roomServices;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeDeviceRepository = new Mock<IHomeDeviceRepository>();
        _homeServices = new Mock<IHomeServices>();
        _deviceServices = new Mock<IDeviceServices>();
        _homeMemberService = new Mock<IHomeMemberService>();
        _roomServices = new Mock<IRoomServices>();
        _homeDeviceServices = new HomeDeviceServices(_homeDeviceRepository.Object, _homeServices.Object, _deviceServices.Object, 
            _homeMemberService.Object, _roomServices.Object);
    }
    
        
    [TestMethod]
    public void Create_WhenCalled_ShouldCreateHomeDevice()
    {
        var device = new Mock<ADevice>().Object;
        var home = new Mock<AHome>().Object;
        
        var result = _homeDeviceServices.Create(device, home);
        
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void AddHomeDevice_WhenCalled_ShouldAddHomeDevice()
    {
        var homeId = 1;
        var deviceId = 1;
        var userId = 1;
        
        var home = new Mock<AHome>().Object;
        var device = new Mock<ADevice>().Object;
        var homeDevice = new Mock<HomeDevice>();
        homeDevice.Setup(x => x.Device).Returns(device);
        
        _homeServices.Setup(x => x.GetHome(homeId)).Returns(home);
        _deviceServices.Setup(x => x.GetDevice(deviceId)).Returns(device);
        
        _homeDeviceServices.AddHomeDevice(homeId, deviceId, userId);
        
        _homeDeviceRepository.Verify(x => x.Create(It.IsAny<HomeDevice>()), Times.Once);
        _homeMemberService.Verify(x => x.GetHomeMember(homeId, userId), Times.Once);
        _homeMemberService.Verify(x => x.CanAddDevices(It.IsAny<AHomeMember>()), Times.Once);
    }
    
    [TestMethod]
    public void AssignDeviceToRoom_ShouldThrowExceptionWhenUserIsNotOwner()
    {
        var homeId = 1;
        var deviceId = new Guid();
        var roomId = 1;
        var userId = 1;
        
        var room = new Mock<ARoom>();
        room.Setup(x => x.HomeId).Returns(homeId);
        room.Setup(x => x.Home).Returns(new Mock<AHome>().Object);
        room.Setup(x => x.Devices).Returns(new List<AHomeDevice>());
        
        _roomServices.Setup(x => x.GetRoom(roomId)).Returns(room.Object);
        _homeServices.Setup(x => x.IsOwner(homeId, userId)).Returns(false);
        
        Assert.ThrowsException<UnauthorizedAccessException>(() => _homeDeviceServices.AssignDeviceToRoom(homeId, deviceId, roomId, userId));
    }
    
    [TestMethod]
    public void AssignDeviceToRoom_ShouldThrowExceptionWhenRoomDoesNotBelongToHome()
    {
        var homeId = 1;
        var deviceId = new Guid();
        var roomId = 1;
        var userId = 1;
        
        var room = new Mock<ARoom>();
        room.Setup(x => x.HomeId).Returns(2);
        room.Setup(x => x.Home).Returns(new Mock<AHome>().Object);
        room.Setup(x => x.Devices).Returns(new List<AHomeDevice>());
        
        _roomServices.Setup(x => x.GetRoom(roomId)).Returns(room.Object);
        _homeServices.Setup(x => x.IsOwner(homeId, userId)).Returns(true);
        
        Assert.ThrowsException<BadRequestException>(() => _homeDeviceServices.AssignDeviceToRoom(homeId, deviceId, roomId, userId));
    }
    
    [TestMethod]
    public void GetDevice_ShouldThrowExceptionWhenDeviceIsNull()
    {
        var deviceId = new Guid();
        
        _homeDeviceRepository.Setup(x => x.GetById(deviceId)).Returns((HomeDevice)null);
        
        Assert.ThrowsException<NotFoundException>(() => _homeDeviceServices.GetDevice(deviceId));
    }
    
    [TestMethod]
    public void GetDevice_ShouldReturnDevice1()
    {
        var deviceId = Guid.NewGuid();
        
        var device = new Mock<HomeDevice>().Object;
        
        _homeDeviceRepository.Setup(x => x.GetById(deviceId)).Returns(device);
        
        var result = _homeDeviceServices.GetDevice(deviceId);
        
        Assert.AreEqual(device, result);
    }
    
    [TestMethod]
    public void ListDeviceInRoom_ShouldReturnDevices()
    {
        var room = new Mock<ARoom>().Object;
        
        var result = _homeDeviceServices.ListDeviceInRoom(room);
        
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public void AssignDeviceToRoom_ShouldAssignDeviceToRoom()
    {
        var homeId = 1;
        var deviceId = new Guid();
        var roomId = 1;
        var userId = 1;
        
        var room = new Mock<ARoom>();
        room.Setup(x => x.HomeId).Returns(homeId);
        room.Setup(x => x.Home).Returns(new Mock<AHome>().Object);
        room.Setup(x => x.Devices).Returns(new List<AHomeDevice>());
        
        var homeDevice = new Mock<HomeDevice>();
        
        _roomServices.Setup(x => x.GetRoom(roomId)).Returns(room.Object);
        _homeServices.Setup(x => x.IsOwner(homeId, userId)).Returns(true);
        _homeDeviceRepository.Setup(x => x.GetById(deviceId)).Returns(homeDevice.Object);
        
        _homeDeviceServices.AssignDeviceToRoom(homeId, deviceId, roomId, userId);
        
        _homeDeviceRepository.Verify(x => x.SaveChanges(), Times.Once);
    }
    
    [TestMethod]
    public void CreateEvent_ShouldCreateEvent()
    {
        var deviceId = new Guid();
        var eventType = EventType.TurnOn;
        
        var device = new Mock<HomeDevice>();
        
        _homeDeviceRepository.Setup(x => x.GetById(deviceId)).Returns(device.Object);
        
        _homeDeviceServices.CreateEvent(deviceId, eventType);
        
        device.Verify(x => x.GenerateEvent(eventType), Times.Once);
        _homeDeviceRepository.Verify(x => x.SaveChanges(), Times.Once);
    }
    
    [TestMethod]
    public void ChangeName_ShouldChangeName()
    {
        var deviceId = new Guid();
        var newName = "newName";
        
        var device = new Mock<HomeDevice>();
        
        _homeDeviceRepository.Setup(x => x.GetById(deviceId)).Returns(device.Object);
        
        _homeDeviceServices.ChangeName(deviceId, newName);
        
        _homeDeviceRepository.Verify(x => x.SaveChanges(), Times.Once);
    }
}
