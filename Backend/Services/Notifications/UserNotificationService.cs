using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Notifications;
using ServicesInterfaces.Users;

namespace Services.Notifications;

public class UserNotificationService : IUserNotificationService
{
    private INotificationServices _notificationService;
    private IHomeUserService _homeUserService;
    
    public UserNotificationService(INotificationServices notificationService, IHomeUserService homeUserService)
    {
        _notificationService = notificationService;
        _homeUserService = homeUserService;
    }
    
    public void AddNotificationToUser(int userId, int notificationId)
    {
        ANotification notification = _notificationService.GetNotification(notificationId);
        AHomeUser user = _homeUserService.GetHomeUser(userId);
        user.Notifications.Add(notification);
        _homeUserService.Update(user);
    }
    
    
}