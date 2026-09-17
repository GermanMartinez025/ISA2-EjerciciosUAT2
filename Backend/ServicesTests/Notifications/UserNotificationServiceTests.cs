using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Moq;
using Services.Companys;
using Services.Notifications;
using ServicesInterfaces.Notifications;
using ServicesInterfaces.Users;

namespace ServicesTests.Notifications;

[TestClass]
public class UserNotificationServiceTests
{
    private Mock<INotificationServices> _notificationServices;
    private Mock<IHomeUserService> _homeUserService;
    private Mock<ANotification> _notification;
    private Mock<AHomeUser> _homeUser;
    private UserNotificationService _userNotificationService;
    private List<ANotification> _notifications;
        
    [TestInitialize]
    public void Setup()
    {
        _notificationServices = new Mock<INotificationServices>();
        _homeUserService = new Mock<IHomeUserService>();
        _notification = new Mock<ANotification>();
        _notification.Setup(x => x.Id).Returns(1);
        _notifications = new List<ANotification>();
        _homeUser = new Mock<AHomeUser>();
        _homeUser.Setup(x => x.UserId).Returns(1);
        _homeUser.Setup(x => x.Notifications).Returns(_notifications);
        _userNotificationService = new UserNotificationService(_notificationServices.Object, _homeUserService.Object);
    }
    
    [TestMethod]
    public void AddNotificationToUserTest()
    {
        _notificationServices.Setup(x => x.GetNotification(It.IsAny<int>())).Returns(_notification.Object);
        _homeUserService.Setup(x => x.GetHomeUser(It.IsAny<int>())).Returns(_homeUser.Object);

        _userNotificationService.AddNotificationToUser(1, 1);

        Assert.AreEqual(1, _notifications.Count);
        Assert.AreEqual(_notification.Object, _notifications[0]);
        _homeUserService.Verify(x => x.Update(_homeUser.Object), Times.Once);
    }

    
}


