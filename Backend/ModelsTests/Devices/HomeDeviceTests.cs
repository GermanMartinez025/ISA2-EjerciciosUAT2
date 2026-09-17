using ModelException;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using Models.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class HomeDeviceTests
{
    
    private Mock<AHome> _home;
    private Mock<ADevice> _device;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Mock<AHome>();
        _device = new Mock<ADevice>();
    }
    
    
    [TestMethod]
    public void ParametrizedHomeDeviceConstructor_WhenCalled_CreatesNewHomeDevice()
    {
        var id = Guid.NewGuid();
        var homeDevice = new HomeDevice(_device.Object, _home.Object){Online = true, HardwareId = id, HomeId = 1, DeviceId = 1, Name = "name"};
        
        Assert.AreEqual(homeDevice.HardwareId, id);
        Assert.AreEqual(homeDevice.HomeId, 1);
        Assert.AreEqual(homeDevice.DeviceId, 1);
        Assert.AreEqual(homeDevice.Name, "name");
        Assert.IsNotNull(homeDevice.Home);
        Assert.IsNotNull(homeDevice.Device);
        Assert.IsTrue(homeDevice.Online);
    }
    
    [TestMethod]
    public void NoParametrizedHomeDeviceConstructor_WhenCalled_CreatesNewHomeDevice()
    {
        var homeDevice = new HomeDevice();
        
        Assert.IsNotNull(homeDevice);
    }
    
    [TestMethod]
    public void Validate_WhenDeviceIsNull_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<BadRequestException>(() => new HomeDevice(null, _home.Object));
    }
    
    [TestMethod]
    public void Validate_WhenHomeIsNull_ThrowsArgumentNullException()
    {
        Assert.ThrowsException<BadRequestException>(() => new HomeDevice(_device.Object, null));
    }


    [TestMethod]
    public void GenerateEvent_WhenItsPossible_CallsNotifyMember()
    {
        var homeDevice = new HomeDevice(_device.Object, _home.Object){Online = true};
        _device.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        
        homeDevice.GenerateEvent(EventType.MovementDetection);
        
        _home.Verify(h => h.NotifyMembers(homeDevice, EventType.MovementDetection), Times.Once);
        
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsOffline_ThrowsException()
    {
        var homeDevice = new HomeDevice(_device.Object, _home.Object){Online = false};
        _device.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        
        Assert.ThrowsException<ConflictException>(() => homeDevice.GenerateEvent(EventType.MovementDetection));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsCameraAndEventIsNotSupported_ThrowsException()
    {
        var camera = new Mock<ACamera>();
        camera.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(false);
        
        
        var homeDevice = new HomeDevice(camera.Object, _home.Object){Online = true};
        
        Assert.ThrowsException<BadRequestException>(() => homeDevice.GenerateEvent(EventType.MovementDetection));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsSensorAndEventIsNotSupported_ThrowsException()
    {
        var sensor = new Mock<ASensor>();
        sensor.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(false);
        
        var homeDevice = new HomeDevice(sensor.Object, _home.Object){Online = true};
        
        Assert.ThrowsException<BadRequestException>(() => homeDevice.GenerateEvent(EventType.MovementDetection));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsSensorAndEventIsSupported_CallsNotifyMember()
    {
        var sensor = new Mock<ASensor>();
        sensor.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        
        var homeDevice = new HomeDevice(sensor.Object, _home.Object){Online = true};
        
        homeDevice.GenerateEvent(EventType.MovementDetection);
        
        _home.Verify(h => h.NotifyMembers(homeDevice, EventType.MovementDetection), Times.Once);
    }
    
    [TestMethod]
    public void ParametrizedConstructor_WhenCalled_SetRoom()
    {
        var homeDevice = new HomeDevice(_device.Object, _home.Object);
        
        Assert.IsNull(homeDevice.Room);
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsSmartLampAndLastEventIsEqualToEvent_ThrowsException()
    {
        var smartLamp = new Mock<ASmartLamp>();
        smartLamp.Setup(x => x.SupportEvent(EventType.TurnOn)).Returns(true);
        smartLamp.Setup(x => x.CanRepeatLastEvent).Returns(false);
        
        var homeDevice = new HomeDevice(smartLamp.Object, _home.Object){Online = true};
        
        homeDevice.GenerateEvent(EventType.TurnOn);
        
        Assert.ThrowsException<ConflictException>(() => homeDevice.GenerateEvent(EventType.TurnOn));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsCameraAndLastEventIsEqualToEvent_NotThrowsException()
    {
        var camera = new Mock<ACamera>();
        camera.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        camera.Setup(x => x.CanRepeatLastEvent).Returns(true);
        
        var homeDevice = new HomeDevice(camera.Object, _home.Object){Online = true};
        
        homeDevice.GenerateEvent(EventType.MovementDetection);
        homeDevice.GenerateEvent(EventType.MovementDetection);
        
        _home.Verify(h => h.NotifyMembers(homeDevice, EventType.MovementDetection), Times.Exactly(2));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsSensorAndLastEventIsEqualToEvent_TrowsException()
    {
        var sensor = new Mock<ASensor>();
        sensor.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        sensor.Setup(x => x.CanRepeatLastEvent).Returns(false);
        
        var homeDevice = new HomeDevice(sensor.Object, _home.Object){Online = true};
        
        homeDevice.GenerateEvent(EventType.MovementDetection);
        
        Assert.ThrowsException<ConflictException>(() => homeDevice.GenerateEvent(EventType.MovementDetection));
    }
    
    [TestMethod]
    public void GenerateEvent_WhenDeviceIsMotionSensorAndLastEventIsEqualToEvent_NotThrowsException()
    {
        var motionSensor = new Mock<AMotionSensor>();
        motionSensor.Setup(x => x.SupportEvent(EventType.MovementDetection)).Returns(true);
        motionSensor.Setup(x => x.CanRepeatLastEvent).Returns(true);
        
        var homeDevice = new HomeDevice(motionSensor.Object, _home.Object){Online = true};
        
        homeDevice.GenerateEvent(EventType.MovementDetection);
        homeDevice.GenerateEvent(EventType.MovementDetection);
        
        _home.Verify(h => h.NotifyMembers(homeDevice, EventType.MovementDetection), Times.Exactly(2));
    }
    
}