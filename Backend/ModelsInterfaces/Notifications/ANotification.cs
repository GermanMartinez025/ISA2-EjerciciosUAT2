using ModelException;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Users;

namespace ModelInterface.Notifications;

public abstract class ANotification : INotification
{
    public virtual int Id { get; set; }
    public EventType Event { get; set; }

    public AHomeDevice Device { get; set; }
    public AHomeMember User { get; set; }

    public bool Read { get;  set; }
    
    public DateTime Date { get; set; }

    protected ANotification()
    {
    }

    protected ANotification(EventType @event, AHomeDevice device, AHomeMember user)
    {
        Validate(device);
        Event = @event;
        Device = device;
        User = user;
        Read = false;
        Date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ": 00");
    }
   
    
    private void Validate(AHomeDevice device)
    {
        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }
    }

}