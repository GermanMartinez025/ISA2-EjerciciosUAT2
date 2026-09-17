using ModelException;
using ModelInterface.Companys;
using ModelInterface.Notifications;
using Models.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class SensorTests
{
    private Mock<ACompany> _company;

    [TestInitialize]
    public void Setup()
    {
        _company = new Mock<ACompany>();
    }
    
    [TestMethod]
    public void ParametrizedSensorConstructor_WhenCalled_CreatesNewSensor()
    {
        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = "Sensor/Photos";
        
        var sensor = new Sensor(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(sensor.Id, 1);
        Assert.AreEqual(sensor.Name, Name);
        Assert.AreEqual(sensor.ModelNumber, ModelNumber);
        Assert.AreEqual(sensor.Description, Description);
        Assert.AreEqual(sensor.Photos[0].Url, Photos);
    }

    [TestMethod]
    public void NoParametrizedSensor_WhenCalled_CreatesNewSensor()
    {
        var sensor = new Sensor();
        
        Assert.IsNotNull(sensor);
    }

    [TestMethod]
    public void Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var name = "";
        var modelNumber = "1";
        var description = "Valid description";
        var photos = "photo.jpg";
        
        Assert.ThrowsException<BadRequestException>(() => new Sensor(name, modelNumber, description, photos, _company.Object));
    }
    
    [TestMethod]
    public void Validate_ShouldThrowException_WhenDescriptionIsEmpty()
    {
        var name = "Valid name";
        var modelNumber = "1";
        var description = "";
        var photos = "photo.jpg";
        
        Assert.ThrowsException<BadRequestException>(() => new Sensor(name, modelNumber, description, photos, _company.Object));
    }

    [TestMethod]
    public void Validate_ShouldThrowException_WhenPhotosIsEmpty()
    {
        var name = "Valid name";
        var modelNumber = "1";
        var description = "Valid description";
        var photos = "";
        
        Assert.ThrowsException<BadRequestException>(() => new Sensor(name, modelNumber, description, photos, _company.Object));
    }
    
    [TestMethod]
    public void SupportEvent_WhenEventTypeIsStatusOpen_ReturnsTrue()
    {
        var sensor = new Sensor();
        
        var result = sensor.SupportEvent(EventType.StatusOpen);
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void SupportEvent_WhenEventTypeIsStatusClose_ReturnsTrue()
    {
        var sensor = new Sensor();
        
        var result = sensor.SupportEvent(EventType.StatusClose);
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void SupportEvent_WhenEventTypeIsNotStatusOpenOrStatusClose_ReturnsFalse()
    {
        var sensor = new Sensor();
        
        var result = sensor.SupportEvent(EventType.MovementDetection);
        
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void SupportEvent_WhenEventTypeIsNotStatusOpenOrStatusClose_ReturnsFalse2()
    {
        var sensor = new Sensor();
        
        var result = sensor.SupportEvent(EventType.PersonDetection);
        
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void CanRepeatLastEvent_WhenCalled_ReturnsFalse()
    {
        var sensor = new Sensor("Name", "1", "Description", "Photos", _company.Object);
        
        var result = sensor.CanRepeatLastEvent;
        
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void Validate_ShouldThrowException_WhenMainPhotoIsNotInPhotosList()
    {
        var name = "Valid Name";
        var modelNumber = "1";
        var description = "Valid description";
        var photos = new List<string> {"photo1.jpg", "photo2.jpg"};
        var mainPhoto = "photo3.jpg";
        
        Assert.ThrowsException<BadRequestException>(() => new Sensor(name, modelNumber, description, photos, _company.Object, mainPhoto));
    }
    
    [TestMethod]
    public void ParametrizedSensorConstructor_WhenCalledWithMainPhoto_CreatesNewSensor()
    {
        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg"};
        var MainPhoto = "MainPhoto.jpg";
        
        var sensor = new Sensor(Name, ModelNumber, Description, Photos, _company.Object, MainPhoto){Id = 1};
        
        Assert.AreEqual(sensor.Id, 1); 
        Assert.AreEqual(sensor.Name, Name);
        Assert.AreEqual(sensor.ModelNumber, ModelNumber);
        Assert.AreEqual(sensor.Description, Description);
        Assert.AreEqual(sensor.Photos[0].Url, Photos[0]);
        Assert.AreEqual(sensor.MainPhoto, MainPhoto);
    }
    
    [TestMethod]
    public void ParametrizedSensorConstructor_WhenCalledWithPhotosList_CreatesNewSensor()
    {
        var Name = "Sensor";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg", "photo1.jpg"};
        
        var sensor = new Sensor(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(sensor.Id, 1); 
        Assert.AreEqual(sensor.Name, Name);
        Assert.AreEqual(sensor.ModelNumber, ModelNumber);
        Assert.AreEqual(sensor.Description, Description);
        Assert.AreEqual(sensor.Photos[0].Url, Photos[0]);
        Assert.AreEqual(sensor.Photos[1].Url, Photos[1]);
        Assert.AreEqual(sensor.MainPhoto, Photos[0]);
    }

}