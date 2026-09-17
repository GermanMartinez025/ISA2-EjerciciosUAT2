using ModelException;
using ModelInterface.Companys;
using ModelInterface.Notifications;
using Models.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class CameraTests
{
    private Mock<ACompany> _company;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Mock<ACompany>();
    }
    
    [TestMethod]
    public void ParametrizedCameraConstructor_WhenCalled_CreatesNewCamera()
    {
        var Name = "Camera";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = "Camera/Photos";
        var OutsideEnvironment = true;
        var MovementDetection = true;
        var PersonDetection = true;
        
        var camera = new Camera(Name, ModelNumber, Description, Photos, _company.Object,OutsideEnvironment, MovementDetection, PersonDetection){Id = 1};
        
        Assert.AreEqual(camera.Id, 1); 
        Assert.AreEqual(camera.Name, Name);
        Assert.AreEqual(camera.ModelNumber, ModelNumber);
        Assert.AreEqual(camera.Description, Description);
        Assert.AreEqual(camera.Photos[0].Url, Photos);
        Assert.AreEqual(camera.OutsideEnvironment, OutsideEnvironment);
        Assert.AreEqual(camera.MovementDetection, MovementDetection);
        Assert.AreEqual(camera.PersonDetection, PersonDetection);
    }

    [TestMethod]
    public void NoParametrizedCameraConstructor_WhenCalled_CreatesNewCamera()
    {
        var camera = new Camera();
        
        Assert.IsNotNull(camera);
    }
    
    [TestMethod]
    public void Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var name = "";
        var modelNumber = "1";
        var description = "Valid description";
        var photos = "photo.jpg";
        var outsideEnvironment = true;
        var movementDetection = true;
        var personDetection = true;
        
        Assert.ThrowsException<BadRequestException>(() => new Camera(name, modelNumber, description,  photos, _company.Object, outsideEnvironment, movementDetection, personDetection));
    }

    

    [TestMethod]
    public void Validate_ShouldThrowException_WhenDescriptionIsEmpty()
    {
        var name = "Valid Name";
        var modelNumber = "1";
        var description = "";
        var photos = "photo.jpg";
        var outsideEnvironment = true;
        var movementDetection = true;
        var personDetection = true;
        
        Assert.ThrowsException<BadRequestException>(() => new Camera(name, modelNumber, description, photos, _company.Object, outsideEnvironment, movementDetection, personDetection));
    }

    [TestMethod]
    public void Validate_ShouldThrowException_WhenPhotosIsEmpty()
    {
        var name = "Valid Name";
        var modelNumber = "1";
        var description = "Valid description";
        var photos = "";
        var outsideEnvironment = true;
        var movementDetection = true;
        var personDetection = true;
        
        Assert.ThrowsException<BadRequestException>(() => new Camera(name, modelNumber, description, photos, _company.Object, outsideEnvironment, movementDetection, personDetection));
    }

    [TestMethod]
    public void SupportEvent_WhenCalledAndDeviceSupportsEvent_ReturnsTrue()
    {
        
        var camera = new Camera("Name", "1", "Description", "Photos",_company.Object, true, true, true);
        
        var result = camera.SupportEvent(EventType.MovementDetection);
        
        Assert.IsTrue(result);
        
    }
    
    [TestMethod]
    public void SupportEvent_WhenCalledReturnsTrue_IfSupportsPersonDetection()
    {
        var camera = new Camera("Name", "1", "Description", "Photos", _company.Object, true, true, true);
        
        var result = camera.SupportEvent(EventType.PersonDetection);
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void SupportEvent_WhenCalledReturnsFalse_IfDoesNotSupportEvent()
    {
        var camera = new Camera("Name", "1", "Description", "Photos", _company.Object, true, true, true);
        
        var result = camera.SupportEvent(EventType.StatusOpen);
        
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void CanRepeatLastEvent_WhenCalled_ReturnsTrue()
    {
        var camera = new Camera("Name", "1", "Description", "Photos", _company.Object, true, true, true);
        
        var result = camera.CanRepeatLastEvent;
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void ParametrizedCameraConstructor_WhenCalledWithMainPhoto_CreatesNewCamera()
    {
        var Name = "Camera";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg"};
        var OutsideEnvironment = true;
        var MovementDetection = true;
        var PersonDetection = true;
        var MainPhoto = "MainPhoto.jpg";
        
        var camera = new Camera(Name, ModelNumber, Description, Photos, _company.Object,MainPhoto, OutsideEnvironment, MovementDetection, PersonDetection){Id = 1};
        
        Assert.AreEqual(camera.Id, 1); 
        Assert.AreEqual(camera.Name, Name);
        Assert.AreEqual(camera.ModelNumber, ModelNumber);
        Assert.AreEqual(camera.Description, Description);
        Assert.AreEqual(camera.Photos[0].Url, Photos[0]);
        Assert.AreEqual(camera.OutsideEnvironment, OutsideEnvironment);
        Assert.AreEqual(camera.MovementDetection, MovementDetection);
        Assert.AreEqual(camera.PersonDetection, PersonDetection);
        Assert.AreEqual(camera.MainPhoto, MainPhoto);
    }
    
    [TestMethod]
    public void ParametrizedCameraConstructor_WhenCalledWithPhotosList_CreatesNewCamera()
    {
        var Name = "Camera";
        var ModelNumber = "1";
        var Description = "Camera Description";
        var Photos = new List<string>{"MainPhoto.jpg", "photo1.jpg"};
        var OutsideEnvironment = true;
        var MovementDetection = true;
        var PersonDetection = true;
        
        var camera = new Camera(Name, ModelNumber, Description, Photos, _company.Object, OutsideEnvironment, MovementDetection, PersonDetection){Id = 1};
        
        Assert.AreEqual(camera.Id, 1); 
        Assert.AreEqual(camera.Name, Name);
        Assert.AreEqual(camera.ModelNumber, ModelNumber);
        Assert.AreEqual(camera.Description, Description);
        Assert.AreEqual(camera.Photos[0].Url, Photos[0]);
        Assert.AreEqual(camera.OutsideEnvironment, OutsideEnvironment);
        Assert.AreEqual(camera.MovementDetection, MovementDetection);
        Assert.AreEqual(camera.PersonDetection, PersonDetection);
        Assert.AreEqual(camera.MainPhoto, Photos[0]);
    }
    
    
}