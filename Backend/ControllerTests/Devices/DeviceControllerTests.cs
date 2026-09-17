using Controllers.Devices;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ModelsAPI.Devices;
using Moq;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace ControllerTests.Devices;

[TestClass]
public class DeviceControllerTests
{
    
    private Mock<IDeviceServices> _deviceService;
    private DeviceController _deviceController;
    private Mock<ISessionService> _sessionService;
    private Mock<ICompanyOwnerService> _companyOwnerService;
    private Mock<ACompanyOwner> _companyOwner;
    private string _token;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _token = "token";
        _companyOwner = new Mock<ACompanyOwner>();
        _deviceService = new Mock<IDeviceServices>();
        _sessionService = new Mock<ISessionService>();
        _companyOwnerService = new Mock<ICompanyOwnerService>();
        _deviceController = new DeviceController(_deviceService.Object, _companyOwnerService.Object, _sessionService.Object);
        _companyOwner.Setup(s => s.User).Returns(new Mock<AUser>().Object);
        _companyOwner.Setup(s => s.Company).Returns(new Mock<ACompany>().Object);
        _deviceService.Setup(s => s.AddDevice(It.IsAny<RequestCreateDevice>(), It.IsAny<ACompany>())).Returns(new Mock<ADevice>().Object);
    }
    
    [TestMethod]
    public void ParametrizedDeviceControllerConstructor_WhenCalled_CreatesNewDeviceController()
    {
        var deviceController = new DeviceController(_deviceService.Object, _companyOwnerService.Object, _sessionService.Object);
        
        Assert.IsNotNull(deviceController);
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithACamera_ShouldReturnCameraService()
    {
        _sessionService.Setup(s => s.GetUserFromSession(It.IsAny<string>())).Returns(_companyOwner.Object.User);
        _companyOwnerService.Setup(s => s.GetById(It.IsAny<int>())).Returns(_companyOwner.Object);
        _deviceController.CreateDevice(new RequestCreateDevice(){Name = "Camera", ModelNumber = "123", Description = "Camera", Photos = new List<string>() {"Photos"}, OutsideEnvironment = true, MovementDetection = true, PersonDetection = true}, _token);

        _deviceService.Verify(s => s.AddDevice(It.IsAny<RequestCreateDevice>(), It.IsAny<ACompany>()), Times.Once);

    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithASensor_ShouldReturnSensorService()
    {
        _sessionService.Setup(s => s.GetUserFromSession(It.IsAny<string>())).Returns(_companyOwner.Object.User);
        _companyOwnerService.Setup(s => s.GetById(It.IsAny<int>())).Returns(_companyOwner.Object);
        _deviceController.CreateDevice(new RequestCreateDevice(){Name = "Sensor", ModelNumber = "123", Description = "Sensor", Photos = new List<string>() {"Photos"}, OutsideEnvironment = null, MovementDetection = null, PersonDetection = null}, _token);
        
        _deviceService.Verify(s => s.AddDevice(It.IsAny<RequestCreateDevice>(), It.IsAny<ACompany>()), Times.Once);
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalled_ReturnsResponseCreateDevice()
    {
        _companyOwner.Setup(s => s.Company).Returns(new Mock<ACompany>().Object);
        _sessionService.Setup(s => s.GetUserFromSession(It.IsAny<string>())).Returns(_companyOwner.Object.User);
        _companyOwnerService.Setup(s => s.GetById(It.IsAny<int>())).Returns(_companyOwner.Object);
        var response = _deviceController.CreateDevice(new RequestCreateDevice(){Name = "Camera", ModelNumber = "123", Description = "Camera", Photos = new List<string>() {"Photos"}, Type = "Camera",OutsideEnvironment = true, MovementDetection = true, PersonDetection = true}, _token);
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalled_ReturnsResponseGetAllDevices()
    {
        _deviceService.Setup(s => s.GetFilterDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>())).Returns(new List<IDevice>());
        var response = _deviceController.GetFilterDevices(1, 10, null, null, null, null);
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetSupportedDevices_WhenCalled_ReturnsResponseGetTypeDevice()
    {
        var response = _deviceController.GetSupportedDevices();
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalledWithDeviceName_ReturnsResponseGetAllDevices()
    {
        _deviceService.Setup(s => s.GetFilterDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>())).Returns(new List<IDevice>());
        var response = _deviceController.GetFilterDevices(1, 10, new List<string>(){ "Camera" }, null, null, null);
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalledWithDeviceModel_ReturnsResponseGetAllDevices()
    {
        _deviceService.Setup(s => s.GetFilterDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>())).Returns(new List<IDevice>());
        var response = _deviceController.GetFilterDevices(1, 10, null, new List<string>(){ "123" }, null, null);
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalledWithCompanyName_ReturnsResponseGetAllDevices()
    {
        _deviceService.Setup(s => s.GetFilterDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>())).Returns(new List<IDevice>());
        var response = _deviceController.GetFilterDevices(1, 10, null, null, new List<string>(){ "Company" }, null);
        
        Assert.IsNotNull(response);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalledWithDeviceType_ReturnsResponseGetAllDevices()
    {
        _deviceService.Setup(s => s.GetFilterDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>())).Returns(new List<IDevice>());
        var response = _deviceController.GetFilterDevices(1, 10, null, null, null, new List<string>(){ "Camera" });
        
        Assert.IsNotNull(response);
    }
    
}