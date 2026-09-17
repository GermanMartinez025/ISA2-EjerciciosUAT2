using IRepositories.Repositories.NotificationRepositories;
using Microsoft.EntityFrameworkCore;
using ModelInterface.Notifications;
using Models.Notifications;

namespace Repositories.NotificationRepositories;

public class NotificationRepository : INotificationRepository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ANotification> _notifications;
    
    public NotificationRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _notifications = dbContext.Set<ANotification>();
    }
    
    public ANotification Create(ANotification notify)
    {
        _notifications.Add((Notification)notify);
        _dbContext.SaveChanges();

        return notify;
    }
    
    public ANotification? GetById(int id)
    {
        return _notifications.FirstOrDefault(n => n.Id == id);
    }
    
    public List<ANotification> GetAll()
    {
        return new List<ANotification>(_notifications.ToList());
    }
    
}