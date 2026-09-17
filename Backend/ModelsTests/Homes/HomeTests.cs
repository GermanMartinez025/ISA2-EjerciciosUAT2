using ModelException;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Homes;
using Models.Users.UserTypes;
using Moq;

namespace ModelsTests.Homes;

[TestClass]
public class HomeTests
{
    private int _id;
    private string _mainStreet;
    private int _doorNumber;
    private string _name;
    private double _latitude;
    private double _longitude;
    private int _maxMembers;
    private Mock<AHomeUser> _aHomeAMemberWithPhotoMock;
    private AHomeUser _aHomeAMemberWithPhoto;
    private Home _home;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _id = 1;
        _mainStreet = "Main Street";
        _doorNumber = 123;
        _name = "Casita";
        _latitude = 82.123;
        _longitude = -123.123;
        _maxMembers = 3;
        _aHomeAMemberWithPhotoMock = new Mock<AHomeUser>();
        _aHomeAMemberWithPhotoMock.Setup(h => h.UserId).Returns(1);
        _aHomeAMemberWithPhoto = _aHomeAMemberWithPhotoMock.Object;
        
    }
    
    [TestMethod]
    public void NoParametrizedHomeConstructor_WhenCalled_CreatesNewHome()
    {
        _home = new Home();

        Assert.IsNotNull(_home);
    }

    [TestMethod]
    public void ParametrizedHomeConstructor_WhenCalled_CreatesNewHome()
    {
        Home home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto) { Id = _id };
        Assert.AreEqual(home.Id, _id);
        Assert.AreEqual(home.MainStreet, _mainStreet);
        Assert.AreEqual(home.DoorNumber, _doorNumber);
        Assert.AreEqual(home.Name, _name);
        Assert.AreEqual(home.Latitude, _latitude);
        Assert.AreEqual(home.Longitude, _longitude);
        Assert.AreEqual(home.MaxMembers, _maxMembers);
        Assert.AreEqual(home.Members[0].AHomeUser, _aHomeAMemberWithPhoto);
        Assert.AreEqual(home.Owner, _aHomeAMemberWithPhoto);
        Assert.IsNotNull(home.Devices);
        Assert.AreEqual(home.HomeOwnerId, _aHomeAMemberWithPhoto.UserId);
        Assert.AreEqual(home.Members.Count, 1);
        Assert.AreEqual(home.Users.Count, 1);
    }
    
    [TestMethod]
    public void ParametrizedHomeConstructor_WhenHomeOwnerIsNull_ThrowsException()
    {
        _aHomeAMemberWithPhoto = null;

        Assert.ThrowsException<NotFoundException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    [TestMethod]
    public void ParametrizedHomeConstructor_WhenMaxMembersIsLessThanOne_ThrowsException()
    {
        _maxMembers = 0;
        
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
     
    [TestMethod]
    public void ValidateMainStreetNullTest() {
        _mainStreet = null;
        
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    [TestMethod]
    public void ValidateMainStreetEmptyTest() {
        _mainStreet = "  ";
        
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    [TestMethod]
    public void ValidateDoorNumberIsOkTest() {
        _doorNumber = 1;  
        
        _home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);

        Assert.AreEqual(_doorNumber, _home.DoorNumber);
    }
    
    [TestMethod]
    public void ValidateLatitudeIsOkTest()
    {
        _latitude = -75;
        _home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);
    }
    
    [TestMethod]
    public void ValidateLongitude_ValidValue_DoesNotThrowException()
    {
        _longitude = 90;
        
        _home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);
        
        Assert.AreEqual(_home.Longitude, 90);
    }
    
    
    [TestMethod]
    public void ValidateLatitudeIsLessThanNegative90()
    {
        _latitude = -91;
        
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    
    [TestMethod]
    public void ValidateLatitudeIsGreaterThan90()
    {
        _latitude = 91;
        
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    [TestMethod]
    public void ValidateDoorNumberIsLessThanOneTest() {
        _doorNumber = 0;

        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }

    
    [TestMethod]
    public void ValidatelongitudeIsLessThanNegative180()
    {
        _longitude = -181;
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }

    [TestMethod]
    public void ValidateLongitudeIsGreaterThan180()
    {
        _longitude = 181;
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }

    [TestMethod]
    public void ValidateNameNullTest()
    {
        _name= null;
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }
    
    [TestMethod]
    public void ValidateNameEmptyTest()
    {
        _name = "  ";
        Assert.ThrowsException<BadRequestException>(() => new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto));
    }

    [TestMethod]
    public void NotifyMembers_WhenCalled_CallsAddNotification()
    {
        var home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);
        home.Members[0].Notifiable = true;
        var device = new Mock<AHomeDevice>();
        var eventType = EventType.MovementDetection;
        
        home.NotifyMembers(device.Object, eventType);
        
        Assert.AreEqual(home.Members[0].Notifications.Count, 1);
    }
    
    [TestMethod]
    public void NotifyMembers_WhenCalled_CallsAddNotificationForEachMember()
    {
        var home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);
        home.Members[0].Notifiable = true;
        home.Members.Add(new HomeMember(home, _aHomeAMemberWithPhoto));
        home.Members.Add(new HomeMember(home, _aHomeAMemberWithPhoto));
        home.Members[2].Notifiable = true;
        var device = new Mock<AHomeDevice>();
        var eventType = EventType.MovementDetection;
        
        home.NotifyMembers(device.Object, eventType);
        
        Assert.AreEqual(home.Members[0].Notifications.Count, 1);
        Assert.AreEqual(home.Members[1].Notifications.Count, 0);
        Assert.AreEqual(home.Members[2].Notifications.Count, 1);
    }

    [TestMethod]
    public void ParametrizedConstructor_WhenCalled_CreatesNewRoomList()
    {
        var home = new Home(_mainStreet, _doorNumber, _name, _latitude, _longitude, _maxMembers, _aHomeAMemberWithPhoto);
        
        Assert.IsNotNull(home.Rooms);
        
    }
}