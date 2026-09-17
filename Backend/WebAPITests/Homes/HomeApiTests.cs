using ControllersInterfaces.Homes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelInterface.Homes;
using ModelsAPI;
using Models.Homes;
using ModelsAPI.Devices;
using ModelsAPI.Homes;
using ModelsAPI.Users;
using Moq;
using WebAPI.APIs;

namespace WebAPITests.Homes;

[TestClass]
public class HomeApiTests
{
    private Mock<IHomeController> _mockHomeController;
    private HomeAPI _homeApi;

    [TestInitialize]
    public void Setup()
    {
        _mockHomeController = new Mock<IHomeController>();
        _homeApi = new HomeAPI(_mockHomeController.Object);
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "token";
        httpContext.Request.RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
        httpContext.Request.RouteValues["homeId"] = "1";
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _homeApi.ControllerContext = controllerContext;
    }

    [TestMethod]
    public void AddUserToHome_ShouldReturnOk_WhenUserAddedSuccessfully()
    {
        var homeId = 1;
        var request = new RequestAddUserToHome { Email = "test@example.com" };
        var response = new ResponseAddUserToHome();

        _mockHomeController.Setup(h => h.AddUserToHome(homeId, request)).Returns(response);

        var result = _homeApi.AddUserToHome(homeId, request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
    [TestMethod]
    public void UpdateHomeMember_ShouldReturnOk_WhenUserUpdatedSuccessfully()
    {
        var homeId = 1;
        var userId = 1;
        var request = new RequestUpdateHomeMember();
        var response = new ResponseUpdateHomeMember();

        _mockHomeController.Setup(h => h.UpdateHomeMember(homeId, userId, request)).Returns(response);

        var result = _homeApi.UpdateHomeMember(homeId, userId, request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
    [TestMethod]
    public void GetMembers_ShouldReturnOk_WhenMembersAreRetrievedSuccessfully()
    {
        var homeId = 1;
        var response = new ResponseGetMembers();

        _mockHomeController.Setup(h => h.GetMembers(It.IsAny<int>(), It.IsAny<string>())).Returns(response);

        var result = _homeApi.GetHomeMembers();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        
    }
    
    [TestMethod]
    public void CreateHome_ShouldReturnOk_WhenHomeIsCreatedSuccessfully()
    {
        var request = new RequestCreateHome();
        var response = new ResponseCreateHome();

        _mockHomeController.Setup(h => h.CreateHome(request, It.IsAny<string>())).Returns(response);

        var result = _homeApi.CreateHome(request) as GenericResponse;

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    [TestMethod]
    public void GetDevices_ShouldReturnOk_WhenDevicesAreRetrievedSuccessfully()
    {
        var response = new ResponseGetDevices();

        _mockHomeController.Setup(h => h.GetDevices(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string?>())).Returns(response);

        var result = _homeApi.GetDevicesFilter("roomName");

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
    [TestMethod]
    public void CreateDevices_ShouldReturnOk_WhenDevicesAreCreatedSuccessfully()
    {
        var homeId = 1;
        var request = 1;
        var response = new ResponseHomeDevice();

        _mockHomeController.Setup(h => h.CreateDevice(homeId, request, It.IsAny<string>())).Returns(response);

        var result = _homeApi.CreateDevice(homeId, request) as GenericResponse;

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void UpdateDevice_ShouldReturnOk_WhenDeviceIsUpdatedSuccessfully()
    {
        var request = new RequestUpdateHomeDevice();
        var response = new ResponseHomeDevice();
        _mockHomeController.Setup(h => h.UpdateDevice(It.IsAny<int>(), It.IsAny<Guid>(), request, It.IsAny<string>())).Returns(response);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "token";
        httpContext.Request.RouteValues["homeId"] = "1";
        httpContext.Request.RouteValues["hardwareId"] = Guid.NewGuid().ToString();
        _homeApi.ControllerContext = new ControllerContext { HttpContext = httpContext };

        var result = _homeApi.UpdateDevice(request);

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void UpdateName_ShouldReturnOk_WhenNameIsUpdatedSuccessfully()
    {
        var homeId = 1;
        var response = new ResponseGetHome();
        var request = new RequestUpDateHome { Name = "Casita" };

        _mockHomeController.Setup(h => h.UpDateName(homeId, request, It.IsAny<string>())).Returns(response);

        var result = _homeApi.UpdateName(homeId, request);

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void CreateRoom_ShouldReturnOk_WhenRoomIsCreatedSuccessfully()
    {
        var homeId = 1;
        Mock<ARoom> room = new Mock<ARoom>();
        var request = new RequestCreateRoom { Name = "Room" };
        var response = new ResponseCreateRoom(room.Object);

        _mockHomeController.Setup(h => h.CreateRoom(homeId, request, It.IsAny<string>())).Returns(response);

        var result = _homeApi.CreateRoom(homeId, request);

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void AssignDeviceToRoom_ShouldReturnOk_WhenDeviceIsAssignedToRoomSuccessfully()
    {
        var homeId = 1;
        var roomId = 1;
        var hardwareId = Guid.NewGuid();
        var response = new ResponseHomeDevice();

        _mockHomeController.Setup(h => h.AssignDeviceToRoom(homeId, hardwareId, roomId, It.IsAny<string>())).Returns(response);

        var result = _homeApi.AssignDeviceToRoom(homeId, roomId, hardwareId);

        Assert.IsNotNull(result);
        Assert.AreEqual(response, result.Data);
    }
    
    [TestMethod]
    public void GetHomes_ShouldReturnOk_WhenHomesAreRetrievedSuccessfully()
    {
        var response = new ResponseGetHomes();

        _mockHomeController.Setup(h => h.GetHomes(It.IsAny<string>())).Returns(response);

        var result = _homeApi.GetHomes();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }

}