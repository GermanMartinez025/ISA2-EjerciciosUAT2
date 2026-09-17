using IRepositories.CRUD;
using ModelInterface.Notifications;

namespace IRepositories.Repositories.NotificationRepositories;

public interface INotificationRepository : ICreateRepository<ANotification>, IGetRepository<ANotification>
{
    
}