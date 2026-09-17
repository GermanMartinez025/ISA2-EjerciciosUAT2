using Controllers.Devices;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using ModelsAPI.Devices;
using Moq;
using ServicesInterfaces.Devices;

namespace ControllerTests.Devices;

[TestClass]
public class HomeDeviceControllerTests
{
    private Mock<IHomeDeviceServices> _homeDeviceService;
    private HomeDeviceController _homeDeviceController;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _homeDeviceService = new Mock<IHomeDeviceServices>();
        _homeDeviceController = new HomeDeviceController(_homeDeviceService.Object);
    }
    
    [TestMethod]
    public void CallCreateEvent_WhenSendEventIsCalled()
    {
        var hardwareId = Guid.NewGuid().ToString();
        var request = new RequestEvent { EventType = "MovementDetection" };
        
        _homeDeviceController.SendEvent(hardwareId, request);
        
        _homeDeviceService.Verify(h => h.CreateEvent(It.IsAny<Guid>(), It.IsAny<EventType>()), Times.Once);
    }
    
    
}