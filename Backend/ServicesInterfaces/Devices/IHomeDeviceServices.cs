using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;

namespace ServicesInterfaces.Devices;

public interface IHomeDeviceServices
{
    public AHomeDevice Create(ADevice device, AHome home);
    public AHomeDevice AddHomeDevice(int homeId, int deviceId, int userId);

    public void CreateEvent(Guid deviceId, EventType eventType);
    public AHomeDevice ChangeName(Guid deviceId, string newName);
    public AHomeDevice GetDevice(Guid deviceId);
    public AHomeDevice AssignDeviceToRoom(int homeId, Guid deviceId, int roomId, int userId);
    public List<AHomeDevice> ListDeviceInRoom(ARoom room);
}