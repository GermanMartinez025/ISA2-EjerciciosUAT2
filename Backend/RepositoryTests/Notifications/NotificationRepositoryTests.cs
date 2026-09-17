using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using ModelInterface.Users;
using Models.Devices;
using Models.Homes;
using Models.Notifications;
using Moq;
using Repositories.NotificationRepositories;

namespace RepositoryTests.Notifications;

[TestClass]
public class NotificationRepositoryTests
{
    private Notification _notification;
    private Mock<DbContext> _dbContext;
    private Mock<AHomeMember> _aHomeMember;
    private IQueryable<Notification> _data;
    private Mock<DbSet<ANotification>> _mockSet;
    private NotificationRepository _notificationRepository;
    private Mock<AHomeDevice> _device;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _aHomeMember = new Mock<AHomeMember>();
        _device = new Mock<AHomeDevice>();
        _notification = new Notification(EventType.MovementDetection, _device.Object, _aHomeMember.Object){Id = 1};
        _data = new List<Notification>
        {
            _notification
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<ANotification>>();
        
        _mockSet.As<IQueryable<Notification>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Notification>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Notification>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Notification>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<ANotification>()).Returns(_mockSet.Object);
        _notificationRepository = new NotificationRepository(_dbContext.Object);
    }
    
    [TestMethod]
    public void CreateNotification_ShouldAddNewNotification()
    {
        _notificationRepository.Create(_notification);
        
        _mockSet.Verify(m => m.Add(It.IsAny<Notification>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());   
    }
    
    [TestMethod]
    public void GetById_ShouldReturnNotification()
    {
        var notification = _notificationRepository.GetById(1);
        
        Assert.AreEqual(_notification, notification);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllNotifications()
    {
        var notifications = _notificationRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), notifications.Count);
    }
    
    
}