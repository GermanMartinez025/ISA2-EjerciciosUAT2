namespace ServicesInterfaces.Notifications;

public interface IUserNotificationService
{
    void AddNotificationToUser(int userId, int notificationId);
}