using IRepositories.Repositories.NotificationRepositories;
using ModelException;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using Models.Notifications;
using ServicesInterfaces.Notifications;

namespace Services.Notifications;

public class NotificationServices : INotificationServices
{
    private readonly INotificationRepository _notificationRepository;
    
    public NotificationServices(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public void AddNotification(ANotification notification)
    {
        _notificationRepository.Create(notification);
    }
    
    public ANotification GetNotification(int id)
    {
        ANotification notification = _notificationRepository.GetById(id);

        ValidateNotificationNotNull(notification);

        return notification;
    }
    
    public List<ANotification> GetAllNotifications()
    {
        return _notificationRepository.GetAll();
    }

    private void ValidateNotificationNotNull(ANotification notification)
    {
        if (notification == null)
        {
            throw new BadRequestException("Notification not found");
        }
    }
    
}