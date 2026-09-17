using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using Models.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class SmartLampTests
{
    private Mock<ACompany> _company;

    [TestInitialize]
    public void Setup()
    {
        _company = new Mock<ACompany>();
    }
    
    [TestMethod]
    public void ParametrizedSmartLampConstructor_WhenCalled_CreatesNewSmartLamp()
    {
        var Name = "SmartLamp";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = "SmartLamp/Photos";
        
        var smartLamp = new SmartLamp(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(1, smartLamp.Id);
        Assert.AreEqual(Name, smartLamp.Name);
        Assert.AreEqual(ModelNumber, smartLamp.ModelNumber);
        Assert.AreEqual(Description, smartLamp.Description);
        Assert.AreEqual(Photos, smartLamp.Photos[0].Url);
        Assert.AreEqual(DeviceEnum.SmartLamp, smartLamp.DeviceType);
    }

    [TestMethod]
    public void NoParametrizedSmartLamp_WhenCalled_CreatesNewSmartLamp()
    {
        var smartLamp = new SmartLamp();
        
        Assert.IsNotNull(smartLamp);
    }

    [TestMethod]
    public void SupportEvent_ShouldReturnTrue_WhenEventTypeIsTurnOn()
    {
        var smartLamp = new SmartLamp();

        Assert.IsTrue(smartLamp.SupportEvent(EventType.TurnOn));
    }
   
    [TestMethod]
    public void SupportEvent_ShouldReturnTrue_WhenEventTypeIsTurnOff()
    {
        var smartLamp = new SmartLamp();

        Assert.IsTrue(smartLamp.SupportEvent(EventType.TurnOff));
    }
    
    [TestMethod]
    public void SupportEvent_ShouldReturnFalse_WhenEventTypeIsNotTurnOnOrTurnOff()
    {
        var smartLamp = new SmartLamp();

        Assert.IsFalse(smartLamp.SupportEvent(EventType.MovementDetection));
    }

    [TestMethod]
    public void CanRepeatLastEvent_WhenCalled_ReturnsFalse()
    {
        var smartLamp = new SmartLamp("Name", "1", "Description", "Photos", _company.Object);

        var result = smartLamp.CanRepeatLastEvent;

        Assert.IsFalse(result);
    }
 
    [TestMethod]
    public void ParametrizedSmartLampConstructor_WhenCalledWithMainPhoto_CreatesNewSmartLamp()
    {
        var Name = "SmartLamp";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg"};
        var MainPhoto = "MainPhoto.jpg";
        
        var smartLamp = new SmartLamp(Name, ModelNumber, Description, Photos, _company.Object, MainPhoto){Id = 1};
        
        Assert.AreEqual(1, smartLamp.Id);
        Assert.AreEqual(Name, smartLamp.Name);
        Assert.AreEqual(ModelNumber, smartLamp.ModelNumber);
        Assert.AreEqual(Description, smartLamp.Description);
        Assert.AreEqual(Photos[0], smartLamp.Photos[0].Url);
        Assert.AreEqual(MainPhoto, smartLamp.MainPhoto);
        Assert.AreEqual(DeviceEnum.SmartLamp, smartLamp.DeviceType);
    }
    
    [TestMethod]
    public void ParametrizedSmartLampConstructor_WhenCalledWithPhotosList_CreatesNewSmartLamp()
    {
        var Name = "SmartLamp";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg", "photo1.jpg"};
        
        var smartLamp = new SmartLamp(Name, ModelNumber, Description, Photos, _company.Object){Id = 1};
        
        Assert.AreEqual(1, smartLamp.Id);
        Assert.AreEqual(Name, smartLamp.Name);
        Assert.AreEqual(ModelNumber, smartLamp.ModelNumber);
        Assert.AreEqual(Description, smartLamp.Description);
        Assert.AreEqual(Photos[0], smartLamp.Photos[0].Url);
        Assert.AreEqual(Photos[1], smartLamp.Photos[1].Url);
        Assert.AreEqual(DeviceEnum.SmartLamp, smartLamp.DeviceType);
        Assert.AreEqual(Photos[0], smartLamp.MainPhoto);
    }
}