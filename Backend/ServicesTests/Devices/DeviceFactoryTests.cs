using ModelException;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelsAPI.Devices;
using Moq;
using Services.Devices;

namespace ServicesTests.Devices;

[TestClass]
public class DeviceFactoryTests
{
    private Mock<ACompany> _company;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Mock<ACompany>();
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithACamera_ShouldReturnACamera()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Name",
            ModelNumber = "model",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "Camera",
            OutsideEnvironment = true,
            MovementDetection = true,
            PersonDetection = true
        };
        
        var device = FabricDevice.CreateDevice(request, _company.Object);
        
        Assert.IsInstanceOfType(device, typeof(ACamera));
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithASensor_ShouldReturnASensor()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Name",
            ModelNumber = "model",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "Sensor",
            OutsideEnvironment = null,
            MovementDetection = null,
            PersonDetection = null
        };
        
        var device = FabricDevice.CreateDevice(request, _company.Object);
        
        Assert.IsInstanceOfType(device, typeof(ASensor));
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithAMotionSensor_ShouldReturnAMotionSensor()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Name",
            ModelNumber = "model",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "MotionSensor",
            OutsideEnvironment = null,
            MovementDetection = null,
            PersonDetection = null
        };
        
        var device = FabricDevice.CreateDevice(request, _company.Object);
        
        Assert.IsInstanceOfType(device, typeof(AMotionSensor));
    }
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithASmartLamp_ShouldReturnASmartLamp()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Name",
            ModelNumber = "model",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "SmartLamp",
            OutsideEnvironment = null,
            MovementDetection = null,
            PersonDetection = null
        };
        
        var device = FabricDevice.CreateDevice(request, _company.Object);
        
        Assert.IsInstanceOfType(device, typeof(ASmartLamp));
    }
    
    
    [TestMethod]
    public void CreateDevice_WhenCalledWithNoAvailableDevice_ShouldThrowException()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Name",
            ModelNumber = "model",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            OutsideEnvironment = false,
            MovementDetection = false,
            PersonDetection = false
        };
        
        Assert.ThrowsException<BadRequestException>(() => FabricDevice.CreateDevice(request, _company.Object));
    }

    
}