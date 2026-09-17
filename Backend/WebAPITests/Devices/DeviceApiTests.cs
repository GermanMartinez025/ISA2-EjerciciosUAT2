using ControllersInterfaces.Devices;
using ControllersInterfaces.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelInterface.Devices;
using ModeloValidador.Abstracciones;
using ModelsAPI;
using ModelsAPI.Devices;
using ModelsAPI.ValidatorModels;
using Moq;
using ServicesInterfaces.Devices;
using WebAPI.APIs;

namespace WebAPITests.Devices;

[TestClass]
public class DeviceApiTests
{
    private Mock<IDeviceController> _deviceController;
    private DeviceAPI _deviceApi;
    private Mock<IDeviceController> _deviceControllerMock;
    private Mock<IHomeDeviceController> _homeDeviceController;
    private Mock<IValidationController> _validationController;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeDeviceController = new Mock<IHomeDeviceController>();
        _deviceController = new Mock<IDeviceController>();
        _deviceControllerMock = new Mock<IDeviceController>();
        _validationController = new Mock<IValidationController>();
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "token";
        httpContext.Request.RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
        httpContext.Request.RouteValues["hardwareId"] = "1";
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        httpContext.Request.RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
        httpContext.Request.RouteValues["hardwareId"] = "1";
        _deviceApi = new DeviceAPI(_deviceControllerMock.Object, _homeDeviceController.Object, _validationController.Object);
        _deviceApi.ControllerContext = controllerContext;
    }
    
    [TestMethod]
    public void ParametrizedDeviceApiConstructor_WhenCalled_CreatesNewDeviceApi()
    {
        var deviceApi = new DeviceAPI(_deviceController.Object, _homeDeviceController.Object, _validationController.Object);
        
        Assert.IsNotNull(deviceApi);
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalled_ReturnsResponseCreateDevice()
    {
        var request = new RequestCreateDevice();
        var response = new ResponseCreateDevice();

        _deviceControllerMock.Setup(x => x.CreateDevice(request, It.IsAny<string>()))
            .Returns(response);

        var result = _deviceApi.CreateDevice(request);

        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
    [TestMethod]
    public void SendEvent_WhenCalled_DelegatesToHomeDeviceController()
    {
        var request = new RequestEvent();
        _deviceApi.SendEvent(request);
        
        _homeDeviceController.Verify(x => x.SendEvent(It.IsAny<string>(), request), Times.Once);
    }
    
    [TestMethod]
    public void GetDevicesFilter_WhenCalled_ReturnsGenericResponse()
    {
        int page = 1;
        int pageSize = 10;
        List<string> deviceName = new List<string> { "Device1" };
        List<string> deviceModel = new List<string> { "Model1" };
        List<string> companyName = new List<string> { "Company1" };
        List<string> deviceType = new List<string> { "Type1" };

        ResponseGetAllDevices expectedResponse = new ResponseGetAllDevices();
        _deviceControllerMock.Setup(x => x.GetFilterDevices(page, pageSize, deviceName, deviceModel, companyName, deviceType))
            .Returns(expectedResponse);
        
        var result = _deviceApi.GetDevicesFilter(page, pageSize, deviceName, deviceModel, companyName, deviceType);

        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("Devices retrieved successfully", result.Message);
        Assert.AreEqual(expectedResponse, result.Data);
    }
    
    [TestMethod]
    public void GetSupportedDevices_WhenCalled_ReturnsGenericResponse()
    {
        var expectedResponse = new ResponseGetTypeDevice();
        _deviceControllerMock.Setup(x => x.GetSupportedDevices())
            .Returns(expectedResponse);
        
        var result = _deviceApi.GetSupportedDevices();
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("Device types retrieved successfully", result.Message);
        Assert.AreEqual(expectedResponse, result.Data);
    }

    [TestMethod]
    public void GetValidators_WhenCalled_ReturnsValidatorsSuccessfully()
    {
        var expectedResponse = new ResponseModelValidators(new List<IModeloValidador>());
        
        _validationController.Setup(x => x.GetAllValidators())
            .Returns(expectedResponse);
        
        var result = _deviceApi.GetValidators();
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("Validators retrieved successfully", result.Message);
        Assert.AreEqual(expectedResponse, result.Data);
    }
    
    [TestMethod]
    public void GetValidators_WhenNoValidators_ReturnsEmptyResponse()
    {
        var expectedResponse = new ResponseModelValidators(new List<IModeloValidador>());
        
        _validationController.Setup(x => x.GetAllValidators())
            .Returns(expectedResponse);
        
        var result = _deviceApi.GetValidators();
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("Validators retrieved successfully", result.Message);
        Assert.AreEqual(expectedResponse, result.Data);
    }


}