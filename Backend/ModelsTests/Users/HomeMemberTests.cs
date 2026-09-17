using ModelException;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Notifications;
using Models.Users.UserTypes;
using Moq;

namespace ModelsTests.Users;

[TestClass]
public class HomeMemberTests
{
    private Mock<AHome> _home;
    private Mock<AHomeUser> _user;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Mock<AHome>();
        _user = new Mock<AHomeUser>();
        _user.Setup(u => u.UserId).Returns(1);
    }
    
    [TestMethod]
    public void NoParametrizedHomeUserConstructor_WhenCalled_CreatesNewHomeUser()
    {
        var homeUser = new HomeMember();
        
        Assert.IsNotNull(homeUser);
        
    }
    
    [TestMethod]
    public void ParametrizedHomeUserConstructor_WhenCalled_CreatesNewHomeUser()
    {
        
        
        var homeUser = new HomeMember(_home.Object, _user.Object){HomeId = 1, UserId = 1};
        
        Assert.AreEqual(homeUser.AHomeUser.UserId, 1);
        Assert.AreEqual(homeUser.HomeId, 1);
        Assert.AreEqual(homeUser.UserId, 1);
        
    }
    
    [TestMethod]
    public void InvalidHome_ThrowsException()
    {
        Assert.ThrowsException<NotFoundException>(() => new HomeMember(null, _user.Object));
    }
    
    [TestMethod]
    public void InvalidUser_ThrowsException()
    {
        Assert.ThrowsException<NotFoundException>(() => new HomeMember(_home.Object, null));
    }
    
    [TestMethod]
    public void CreateHomeUser_CreatesNewNotificationList()
    {
        var homeUser = new HomeMember(_home.Object, _user.Object);
        
        Assert.IsNotNull(homeUser.Notifications);
    }
    
    [TestMethod]
    public void AddNotification_AddsNotificationToList()
    {
        var homeUser = new HomeMember(_home.Object, _user.Object);
        var notification = new Mock<ANotification>();
        
        homeUser.AddNotification(notification.Object);
        
        Assert.AreEqual(homeUser.Notifications.Count, 1);
    }
    

}