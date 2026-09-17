using Controllers.Homes;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Users;
using Models.Devices;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Devices;
using ModelsAPI.Homes;
using ModelsAPI.Users;
using Moq;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace ControllerTests.Homes;

[TestClass]
public class HomeControllerTests
{
    private Mock<IHomeServices> _homeServices;
    private Mock<IHomeMemberService> _homeUserService;
    private Mock<ISessionService> _sessionService;
    private HomeController _homeController;
    private Mock<User> _user;
    private Mock<IHomeDeviceServices> _homeDeviceServices;
    private Mock<IRoomServices> _roomServices;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeServices = new Mock<IHomeServices>();
        _homeUserService = new Mock<IHomeMemberService>();
        _sessionService = new Mock<ISessionService>();
        _homeDeviceServices = new Mock<IHomeDeviceServices>();
        _roomServices = new Mock<IRoomServices>();
        _user = new Mock<User>();
        _homeController = new HomeController(_homeServices.Object, _homeUserService.Object, _sessionService.Object, _homeDeviceServices.Object, _roomServices.Object);
        _sessionService.Setup(x => x.GetUserFromSession(It.IsAny<string>())).Returns(_user.Object);
        _homeUserService.Setup(x => x.GetDevices(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<string?>())).Returns(new List<AHomeDevice>());
    }

    [TestMethod]
    public void AddUserToHome_WhenCalled_AddUserToHome()
    {
        var homeId = 1;
        var request = new RequestAddUserToHome() { Email = "example@example.com" };

        _homeController.AddUserToHome(homeId, request);
        
        _homeUserService.Verify(x => x.AddUserToHome(homeId, request.Email), Times.Once);
    }
    
    [TestMethod]
    public void UpdateMember_WhenCalled_UpdateMember()
    {
        var homeId = 1;
        var userId = 1;
        var request = new RequestUpdateHomeMember() { Notifiable = true, ListDevices = true, AddDevices = true };
        _homeUserService.Setup(x => x.GetHomeMember(homeId, userId)).Returns(new Mock<AHomeMember>().Object);

        _homeController.UpdateHomeMember(homeId, userId, request);
        
        _homeUserService.Verify(x => x.SetNotifiable(homeId, userId, request.Notifiable.Value), Times.Once);
        _homeUserService.Verify(x => x.SetListDevices(homeId, userId, request.ListDevices.Value), Times.Once);
        _homeUserService.Verify(x => x.SetAddDevices(homeId, userId, request.AddDevices.Value), Times.Once);
    }

    [TestMethod]
    public void UpdateMember_WhenCalledWithNoNotifiable_DoesntUpdateNotifiable()
    {
        var homeId = 1;
        var userId = 1;
        var request = new RequestUpdateHomeMember() 
        { 
            Notifiable = null,
            ListDevices = true, 
            AddDevices = true 
        };
        _homeUserService.Setup(x => x.GetHomeMember(homeId, userId)).Returns(new Mock<AHomeMember>().Object);

        _homeController.UpdateHomeMember(homeId, userId, request);

        _homeUserService.Verify(x => x.SetNotifiable(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);

        _homeUserService.Verify(x => x.SetListDevices(homeId, userId, request.ListDevices.Value), Times.Once);
        _homeUserService.Verify(x => x.SetAddDevices(homeId, userId, request.AddDevices.Value), Times.Once);
    }
    
    [TestMethod]
    public void UpdateMember_WhenCalledWithNoListDevices_DoesntUpdateListDevices()
    {
        var homeId = 1;
        var userId = 1;
        var request = new RequestUpdateHomeMember() 
        { 
            Notifiable = true,
            ListDevices = null, 
            AddDevices = true 
        };
        
        _homeUserService.Setup(x => x.GetHomeMember(homeId, userId)).Returns(new Mock<AHomeMember>().Object);

        _homeController.UpdateHomeMember(homeId, userId, request);

        _homeUserService.Verify(x => x.SetListDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);

        _homeUserService.Verify(x => x.SetNotifiable(homeId, userId, request.Notifiable.Value), Times.Once);
        _homeUserService.Verify(x => x.SetAddDevices(homeId, userId, request.AddDevices.Value), Times.Once);
    }
    
    [TestMethod]
    public void UpdateMember_WhenCalledWithNoAddDevices_DoesntUpdateAddDevices()
    {
        var homeId = 1;
        var userId = 1;
        var request = new RequestUpdateHomeMember() 
        { 
            Notifiable = true,
            ListDevices = true, 
            AddDevices = null 
        };
        
        _homeUserService.Setup(x => x.GetHomeMember(homeId, userId)).Returns(new Mock<AHomeMember>().Object);

        _homeController.UpdateHomeMember(homeId, userId, request);

        _homeUserService.Verify(x => x.SetAddDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);

        _homeUserService.Verify(x => x.SetNotifiable(homeId, userId, request.Notifiable.Value), Times.Once);
        _homeUserService.Verify(x => x.SetListDevices(homeId, userId, request.ListDevices.Value), Times.Once);
    }
    
    [TestMethod]
    public void GetDevices_WhenCalled_GetDevices()
    {
        var homeId = 1;
        var userId = 0;
        var token = "a";
        var room = "room";
        
        _homeController.GetDevices(homeId, token, room);
        
        _homeUserService.Verify(x => x.GetDevices(homeId, userId, room), Times.Once);
    }
    
    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsResponseGetDevices()
    {
        var homeId = 1;
        var userId = 0;
        var token = "a";
        var room = "room";
        var devices = new List<AHomeDevice>();
        var device = new Mock<AHomeDevice>();
        device.Setup(x => x.Device).Returns(new Camera() { Photos = new List<DevicePhoto>()});
        devices.Add(device.Object);
        _homeUserService.Setup(x => x.GetDevices(homeId, userId, room)).Returns(devices);
        
        var result = _homeController.GetDevices(homeId, token, room);
        
        Assert.IsInstanceOfType(result, typeof(ResponseGetDevices));
        
    }
    
    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsResponseGetDevicesWithSameAmountOfDevices()
    {
        var homeId = 1;
        var userId = 0;
        var token = "a";
        var member1 = new Mock<AHomeMember>();
        var member2 = new Mock<AHomeMember>();
        var member3 = new Mock<AHomeMember>();
        member1.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        member2.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        member3.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        var members = new List<AHomeMember>
        {
            member1.Object,
            member2.Object,
            member3.Object
        };
        _homeUserService.Setup(x => x.GetHomeMembers(homeId, userId)).Returns(members);

        var result = _homeController.GetMembers(homeId, token);
        
        Assert.AreEqual(members.Count, result.Members.Count);
    }
    
    [TestMethod]
    public void CreateHome_WhenCalled_CreateHome()
    {
        var homeRequest = new RequestCreateHome
        {
            Name = "Test Home",
            Address = new Address { MainStreet = "Main St", DoorNumber = 123 },
            Geolocation = new Geolocation { Latitude = 40.7128, Longitude = -74.0060 },
            MaxMembers = 5
        };
        var userToken = "token";
        var userId = 1;
        
        var user = new User() {Id = 1};
        _sessionService.Setup(x => x.GetUserFromSession(userToken)).Returns(user);
        var home = new Mock<AHome>();
        home.Setup(x => x.Id).Returns(1);
        home.Setup(x=>x.Owner).Returns(new HomeUser(user));
        _homeServices.Setup(x => x.AddHome(It.IsAny<RequestCreateHome>(), userId)).Returns(home.Object);
        _homeController.CreateHome(homeRequest, userToken);
        
        _homeServices.Verify(x => x.AddHome(It.IsAny<RequestCreateHome>(), userId), Times.Once);
        
    }
    
    [TestMethod]
    public void GetMembers_WhenCalled_GetMembers()
    {
        var homeId = 1;
        var userId = 0;
        var token = "a";
        Mock<AHome> home = new Mock<AHome>();
        home.Setup(x => x.Members).Returns(new List<AHomeMember>());
        _homeUserService.Setup(x => x.GetHomeMembers(homeId, userId)).Returns(new List<AHomeMember>());

        _homeController.GetMembers(homeId, token);
        
        _homeUserService.Verify(x => x.GetHomeMembers(homeId, userId), Times.Once);
    }
    
    [TestMethod]
    public void GetMembers_WhenCalled_ReturnsSameAmountOfMembers()
    {
        var homeId = 1;
        var userId = 0;
        var token = "a";
        var member1 = new Mock<AHomeMember>();
        var member2 = new Mock<AHomeMember>();
        var member3 = new Mock<AHomeMember>();
        member1.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        member2.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        member3.Setup(x => x.AHomeUser).Returns(new HomeUser() {User = new User()});
        var members = new List<AHomeMember>
        {
            member1.Object,
            member2.Object,
            member3.Object
        };
        _homeUserService.Setup(x => x.GetHomeMembers(homeId, userId)).Returns(members);

        var result = _homeController.GetMembers(homeId, token);
        
        Assert.AreEqual(members.Count, result.Members.Count);
    }
    [TestMethod]
    public void CreateDevice_WhenCalled_CreateOnHome()
    {
        
        var homeId = 1;
        var deviceId = 1;
        var userToken = "token";
        var userId = 1;
        var mockDevice = new Mock<AHomeDevice>();
        var mockDeviceDetails = new Mock<ADevice>();
        mockDevice.Setup(d => d.Device).Returns(mockDeviceDetails.Object);
        mockDeviceDetails.Setup(d => d.Id).Returns(deviceId);
        _sessionService.Setup(x => x.GetUserFromSession(userToken)).Returns(new HomeUser(new User() ){Id = 1}.User);        
        _homeDeviceServices.Setup(x => x.AddHomeDevice(homeId, deviceId, userId)).Returns(mockDevice.Object);
        var result = _homeController.CreateDevice(homeId, deviceId, userToken);
        _homeDeviceServices.Verify(x => x.AddHomeDevice(homeId, deviceId, userId), Times.Once);

    }
    
    [TestMethod]
    public void UpdateDevice_WhenRequestIsValid_CallsChangeName()
    {
        var homeId = 1;
        var hardwareId = Guid.NewGuid();
        var token = "valid_token";
        var request = new RequestUpdateHomeDevice { Name = "New Device Name" };

        var mockDevice = new Mock<AHomeDevice>();
        mockDevice.SetupAllProperties();
        mockDevice.Object.Device = new Mock<ADevice>().Object;
        mockDevice.Object.Name = "Old Device Name";
        mockDevice.Object.Online = true;

        _homeUserService.Setup(s => s.CanUpdateDevices(It.IsAny<AHomeMember>()));
        _homeUserService.Setup(s => s.GetHomeMember(homeId, It.IsAny<int>())).Returns(new Mock<AHomeMember>().Object);
        _sessionService.Setup(s => s.GetUserFromSession(token)).Returns(new Mock<User>().Object);
        _homeDeviceServices.Setup(s => s.GetDevice(hardwareId)).Returns(mockDevice.Object);
        _homeDeviceServices.Setup(s => s.ChangeName(hardwareId, request.Name)).Returns(mockDevice.Object);
        
        var response = _homeController.UpdateDevice(homeId, hardwareId, request, token);
        
        _homeDeviceServices.Verify(s => s.ChangeName(hardwareId, request.Name), Times.Once);
    }

    [TestMethod]
    public void CreateRoom_WhenCalled_CallsAddRoomToHomeOnce()
    {
        var homeId = 1;
        var token = "valid_token";
        var request = new RequestCreateRoom { Name = "Living Room" };
        var mockRoom = new Mock<ARoom>();
    
        _sessionService.Setup(x => x.GetUserFromSession(token)).Returns(new User { Id = 123 });
        _roomServices.Setup(x => x.AddRoomToHome(homeId, request.Name)).Returns(mockRoom.Object);

        var result = _homeController.CreateRoom(homeId, request, token);

        _roomServices.Verify(x => x.AddRoomToHome(homeId, request.Name), Times.Once);
        Assert.IsInstanceOfType(result, typeof(ResponseCreateRoom));
    }
    
    [TestMethod]
    public void AssignDeviceToRoom_WhenCalled_AssignsDeviceToRoomOnce()
    {
        var homeId = 1;
        var hardwareId = Guid.NewGuid();
        var roomId = 1;
        var token = "valid_token";
        var userId = 123;

        var mockDevice = new Mock<AHomeDevice>();
        mockDevice.Setup(d => d.Device).Returns(new Mock<ADevice>().Object); // Configurar el Device
        mockDevice.Setup(d => d.Device.Id).Returns(1);
        mockDevice.Setup(d => d.HomeId).Returns(homeId);
        mockDevice.Setup(d => d.Device.PhotosUrls).Returns(new List<string>());

        _sessionService.Setup(x => x.GetUserFromSession(token)).Returns(new User { Id = userId });
        _homeDeviceServices.Setup(x => x.AssignDeviceToRoom(homeId, hardwareId, roomId, userId)).Returns(mockDevice.Object);
        
        var result = _homeController.AssignDeviceToRoom(homeId, hardwareId, roomId, token);
        
        _homeDeviceServices.Verify(x => x.AssignDeviceToRoom(homeId, hardwareId, roomId, userId), Times.Once);
        Assert.IsInstanceOfType(result, typeof(ResponseHomeDevice));
    }

    [TestMethod]
    public void GetMyHomes_WhenCalled_ReturnsMyHomes()
    {
        _homeServices.Setup(x => x.GetMyHomes(It.IsAny<int>())).Returns(new List<AHome>());
        
        _homeController.GetHomes("token");
        
        _homeServices.Verify(x => x.GetMyHomes(It.IsAny<int>()), Times.Once);
    }
    
}