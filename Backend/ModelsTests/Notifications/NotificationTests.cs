using ModelException;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using ModelInterface.Users;
using Models.Devices;
using Models.Notifications;
using Moq;

namespace ModelsTests.Notifications;

[TestClass]
public class NotificationTests
{
    private Mock<AHomeDevice> _device;
    private EventType _eventType;
    private Mock<AHomeMember> _aHomeMember;


    [TestInitialize]
    public void Initialize()
    {
        _aHomeMember = new Mock<AHomeMember>();
        _device = new Mock<AHomeDevice>();
        _eventType = EventType.MovementDetection;
    }

    [TestMethod]
    public void ParametrizedNotificationConstructor_WhenCalled_CreatesNewNotification()
    {
        var notification = new Notification(_eventType, _device.Object, _aHomeMember.Object);

        Assert.IsNotNull(notification);
        Assert.AreEqual(_eventType, notification.Event);
        Assert.AreEqual(_device.Object, notification.Device);
        Assert.IsTrue(notification.User == _aHomeMember.Object);
        Assert.IsFalse(notification.Read);
        Assert.IsTrue(notification.Date < DateTime.Now);
    }

    [TestMethod]
    public void NoParametrizedNotificationConstructor_WhenCalled_CreatesNewNotification()
    {
        var notification = new Notification();

        Assert.IsNotNull(notification);
    }
    
    [TestMethod]
    public void WhenDeviceIsNull_ThrowsException()
    {
        Assert.ThrowsException<NotFoundException>(() => new Notification(_eventType, null, _aHomeMember.Object));
    }
    
   
}
    
    
    