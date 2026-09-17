using ControllersInterfaces.Devices;
using ModelInterface.Notifications;
using ModelsAPI.Devices;
using ServicesInterfaces.Devices;

namespace Controllers.Devices;

public class HomeDeviceController : IHomeDeviceController
{
    private readonly IHomeDeviceServices _homeDeviceServices;
    
    public HomeDeviceController(IHomeDeviceServices homeDeviceServices)
    {
        _homeDeviceServices = homeDeviceServices;
    }
    
    public void SendEvent(string hardwareId, RequestEvent request)
    {
        var hardwareIdAsGuid = Guid.Parse(hardwareId);
        var eventTypeAsEventType = Enum.Parse<EventType>(request.EventType);
        _homeDeviceServices.CreateEvent(hardwareIdAsGuid,eventTypeAsEventType);
    }
    
}