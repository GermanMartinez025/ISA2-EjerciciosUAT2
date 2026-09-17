using IRepositories.Repositories.NotificationRepositories;
using ModelException;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using Moq;
using Services.Notifications;

namespace ServicesTests.Notifications;

[TestClass]
public class NotificationServicesTests
{
    private Mock<INotificationRepository> _notificationRepository;
    private NotificationServices _notificationServices;
    private Mock<ANotification> _notification;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationServices = new NotificationServices(_notificationRepository.Object);
        _notification = new Mock<ANotification>();
    }
    
    [TestMethod]
    public void AddNotificationTest()
    {
        _notificationServices.AddNotification(_notification.Object);
        _notificationRepository.Verify(x => x.Create(_notification.Object), Times.Once);
    }
    
    [TestMethod]
    public void GetNotificationTest()
    {
        _notificationRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_notification.Object);
        var result = _notificationServices.GetNotification(1);
        Assert.AreEqual(_notification.Object, result);
    }
    
    [TestMethod]
    public void GetAllNotificationsTest()
    {
        var notifications = new List<ANotification>();
        _notificationRepository.Setup(x => x.GetAll()).Returns(notifications);
        var result = _notificationServices.GetAllNotifications();
        Assert.AreEqual(notifications, result);
    }
    
    [TestMethod]
    public void GetNotification_NotFound()
    {
        _notificationRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns((ANotification)null);
        Assert.ThrowsException<BadRequestException>(() => _notificationServices.GetNotification(1));
    }
  
    
}