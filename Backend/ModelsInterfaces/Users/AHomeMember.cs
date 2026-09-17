using ModelException;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using ModelInterface.Users.UserType;

namespace ModelInterface.Users;

public abstract class AHomeMember
{
    public AHome Home { get; set; }
    public virtual AHomeUser AHomeUser { get; set; }
    public int HomeId { get; set; }
    public int UserId { get; set; }
    public virtual bool Notifiable { get; set; }
    public bool ListDevices { get; set; }
    public bool AddDevices { get; set; }
    public bool UpdateDevices { get; set; }
    public List<ANotification> Notifications { get; set; } = new List<ANotification>();
    
    public AHomeMember()
    {
    }
    
    public AHomeMember(AHome home, AHomeUser aHomeUser)
    {
        InvalidDataException(home, aHomeUser);
        Home = home;
        AHomeUser = aHomeUser;
        HomeId = home.Id;
        UserId = aHomeUser.UserId;
        Notifiable = false;
        ListDevices = false;
        AddDevices = false;
        UpdateDevices = false;
        Notifications = new List<ANotification>();
    }
    
    private void InvalidDataException(AHome home, AHomeUser aHomeUser)
    {
        if (home == null)
        {
            throw new NotFoundException("Home cannot be null");
        }
        if (aHomeUser == null)
        {
            throw new NotFoundException("HomeUser cannot be null");
        }
    }

    public void AddNotification(ANotification notification)
    {
        Notifications.Add(notification);
    }
}