using ModelException;
using ModelInterface.Homes;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class AHomeDevice : IHomeDevice
{
    public virtual Guid HardwareId { get; set; }
    public virtual int DeviceId { get; set; }
    public virtual int HomeId { get; set; }
    public virtual int? RoomId { get; set; }
    public ARoom Room { get; set; }
    public virtual ADevice Device { get; set; }
    public AHome Home { get; set; }
    public string Name { get; set; }
    public virtual bool Online { get; set; }
    public EventType? LastEvent { get; set; }

    protected AHomeDevice()
    {
    }
    
    protected AHomeDevice(ADevice device, AHome home)
    {
        Validate(device, home);
        Device = device;
        Home = home;
        Name = device.Name;
        Room = null;
        HardwareId = Guid.NewGuid();
        Online = true;
    }
    
    private void Validate(ADevice device, AHome home)
    {
        if (device == null)
        {
            throw new BadRequestException(nameof(device));
        }
        if (home == null)
        {
            throw new BadRequestException(nameof(home));
        }
        
    }
    
    public virtual void GenerateEvent(EventType eventType)
    {
        if (!Online)
        {
            throw new ConflictException("Device is offline");
        }
        var supportEvent = Device.SupportEvent(eventType);
        if (!supportEvent)
        {
            throw new BadRequestException("Device does not support this event");
        }
        if (LastEvent == eventType && !Device.CanRepeatLastEvent)
        {
            throw new ConflictException("Device can't repeat last event");
        }
        LastEvent = eventType;
        
        Home.NotifyMembers(this, eventType);
    }
    
}