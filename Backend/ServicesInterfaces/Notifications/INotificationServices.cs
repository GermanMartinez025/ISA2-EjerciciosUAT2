using IRepositories.Repositories.NotificationRepositories;
using ModelInterface.Devices;
using ModelInterface.Notifications;

namespace ServicesInterfaces.Notifications;

public interface INotificationServices
{
    public void AddNotification(ANotification notification);
    public ANotification GetNotification(int id);
    public List<ANotification> GetAllNotifications();
}