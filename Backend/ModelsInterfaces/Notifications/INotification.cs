using ModelInterface.Devices;

namespace ModelInterface.Notifications;

public interface INotification
{
    public int Id { get; set; }
    public EventType Event { get; set; }
    public AHomeDevice Device { get; set; }
    public bool Read { get; set; }
    public DateTime Date { get; set; }
}