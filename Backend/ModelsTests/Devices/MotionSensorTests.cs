using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using Models.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class MotionSensorTests
{
    private Mock<ACompany> _company;
    
    [TestInitialize]
    public void Setup()
    {
        _company = new Mock<ACompany>();
    }
    
    [TestMethod]
    public void ParametrizedMotionSensorConstructor_WhenCalled_CreatesNewMotionSensor()
    {

        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = "Sensor/Photos";
        
        var motionSensor = new MotionSensor(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(motionSensor.Id, 1);
        Assert.AreEqual(motionSensor.Name, Name);
        Assert.AreEqual(motionSensor.ModelNumber, ModelNumber);
        Assert.AreEqual(motionSensor.Description, Description);
        Assert.AreEqual(motionSensor.Photos[0].Url, Photos);
        Assert.AreEqual(motionSensor.DeviceType, DeviceEnum.MotionSensor);
    }
  
    
    [TestMethod]
    public void NoParametrizedMotionSensorConstructor_WhenCalled_CreatesNewMotionSensor()
    {
        var motionSensor = new MotionSensor();
        
        Assert.IsNotNull(motionSensor);
    }
    
    [TestMethod]
    public void SupportEvent_ShouldReturnTrue_WhenEventTypeIsMovementDetection()
    {
        var motionSensor = new MotionSensor();

        Assert.IsTrue(motionSensor.SupportEvent(EventType.MovementDetection));
    }
    
    [TestMethod]
    public void SupportEvent_ShouldReturnFalse_WhenEventTypeIsNotMovementDetection()
    {
        var motionSensor = new MotionSensor();

        Assert.IsFalse(motionSensor.SupportEvent(EventType.PersonDetection));
    }
    
    [TestMethod]
    public void CanRepeatLastEvent_WhenCalled_ReturnsTrue()
    {
        var motionSensor = new MotionSensor("Name", "1", "Description", "Photos", _company.Object);
        
        var result = motionSensor.CanRepeatLastEvent;
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void ParametrizedMotionSensorConstructor_WhenCalledWithMainPhoto_CreatesNewMotionSensor()
    {
        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg"};
        var MainPhoto = "MainPhoto.jpg";
        
        var motionSensor = new MotionSensor(Name, ModelNumber, Description, Photos, _company.Object, MainPhoto){Id = 1};
        
        Assert.AreEqual(motionSensor.Id, 1); 
        Assert.AreEqual(motionSensor.Name, Name);
        Assert.AreEqual(motionSensor.ModelNumber, ModelNumber);
        Assert.AreEqual(motionSensor.Description, Description);
        Assert.AreEqual(motionSensor.Photos[0].Url, Photos[0]);
        Assert.AreEqual(motionSensor.MainPhoto, MainPhoto);
        Assert.AreEqual(motionSensor.DeviceType, DeviceEnum.MotionSensor);
    }
    
    [TestMethod]
    public void ParametrizedMotionSensorConstructor_WhenCalledWithPhotosList_CreatesNewMotionSensor()
    {
        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg", "photo1.jpg"};
        
        var motionSensor = new MotionSensor(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(motionSensor.Id, 1); 
        Assert.AreEqual(motionSensor.Name, Name);
        Assert.AreEqual(motionSensor.ModelNumber, ModelNumber);
        Assert.AreEqual(motionSensor.Description, Description);
        Assert.AreEqual(motionSensor.Photos[0].Url, Photos[0]);
        Assert.AreEqual(motionSensor.Photos[1].Url, Photos[1]);
        Assert.AreEqual(motionSensor.DeviceType, DeviceEnum.MotionSensor);
        Assert.AreEqual(motionSensor.MainPhoto, Photos[0]);
    }
}