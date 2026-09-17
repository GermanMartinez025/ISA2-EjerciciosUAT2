using ModelInterface.Devices;
using ModelInterface.Notifications;
using ModelInterface.Users;

namespace Models.Notifications;

public class Notification : ANotification
{
    public Notification()
    {
    }
    
    public Notification(EventType @event, AHomeDevice device, AHomeMember homeMember) : base(@event, device, homeMember)
    {
    }
    
    
}