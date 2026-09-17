using ModelInterface.Homes;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public interface IHomeDevice
{
    public AHome Home { get; set; }
    public ADevice Device { get; set; }
    public bool Online { get; set; }
    public string Name { get; set; }
    public ARoom Room { get; set; }
    public EventType? LastEvent { get; set; }
    
}   