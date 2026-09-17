using ModelsAPI.Devices;

namespace ControllersInterfaces.Devices;

public interface IHomeDeviceController
{
   void SendEvent(string hardwareId, RequestEvent request);
}