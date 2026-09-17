using System.Reflection;
using ModelException;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Devices;
using Models.Users.UserTypes;
using Moq;
using Services.Users;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Users;

namespace ServicesTests.Users;

[TestClass]
public class HomeMemberServiceTests
{
    private Mock<IHomeServices> _homeServices;
    private Mock<IHomeUserService>_userService;
    private Mock<AHome> _home;
    private Mock<AHomeUser> _user; 
    private HomeMemberService _homeMemberService;
    
    [TestInitialize]
    public void Setup()
    {
        _homeServices = new Mock<IHomeServices>();
        _userService = new Mock<IHomeUserService>();
        _home = new Mock<AHome>();
        _user = new Mock<AHomeUser>();
        _homeMemberService = new HomeMemberService(_homeServices.Object, _userService.Object);
    }

    [TestMethod]
    public void Create_ShouldCreateNewHomeUser()
    {
        AHomeMember newAHomeMember = _homeMemberService.Create(_user.Object, _home.Object);
        
        Assert.AreEqual(_user.Object, newAHomeMember.AHomeUser);
        Assert.AreEqual(_home.Object, newAHomeMember.Home);
    }

    [TestMethod]
    public void AddUserToHome_ShouldAddUserToHome()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.MaxMembers).Returns(2);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember>());
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);

        _homeMemberService.AddUserToHome(1, "email@email.com");

        _homeServices.Verify(x => x.Update(_home.Object), Times.Once);
        Assert.AreEqual(_user.Object, _home.Object.Members[0].AHomeUser);
        Assert.AreEqual(_home.Object, _home.Object.Members[0].Home);
    }

    [TestMethod]
    public void AddUserToHome_ShouldThrowExceptionWhenHomeIsFull()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember() });
        _home.Setup(hm => hm.MaxMembers).Returns(0);
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);

        Assert.ThrowsException<ConflictException>(() => _homeMemberService.AddUserToHome(1, "example@example.com"));
    }
    
    [TestMethod]
    public void AddUserToHome_ShouldThrowExceptionWhenUserIsAlreadyMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _home.Setup(hm => hm.MaxMembers).Returns(2);
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        Assert.ThrowsException<ConflictException>(() => _homeMemberService.AddUserToHome(1, "example@example.com"));
    }

    [TestMethod]
    public void SetNotiable_ShouldSetNotifiable()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        _homeMemberService.SetNotifiable(1, 1, true);
        
        Assert.IsTrue(_home.Object.Members[0].Notifiable);
    }

    [TestMethod]
    public void SetNotiable_ShouldThrowExceptionWhenUserIsNotMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        Assert.ThrowsException<NotFoundException>(() => _homeMemberService.SetNotifiable(1, 2, true));
    }

    [TestMethod]
    public void SetCanListDevices_ShouldSetCanListDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        _homeMemberService.SetListDevices(1, 1, true);
        
        Assert.IsTrue(_home.Object.Members[0].ListDevices);
    }
    
    [TestMethod]
    public void SetCanListDevices_ShouldThrowExceptionWhenUserIsNotMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        Assert.ThrowsException<NotFoundException>(() => _homeMemberService.SetListDevices(1, 2, true));
    }

    [TestMethod]
    public void SetAddDevices_ShouldSetAddDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        _homeMemberService.SetAddDevices(1, 1, true);
        
        Assert.IsTrue(_home.Object.Members[0].AddDevices);
    }

    [TestMethod]
    public void SetAddDevices_ShouldThrowExceptionWhenUserIsNotMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        Assert.ThrowsException<NotFoundException>(() => _homeMemberService.SetAddDevices(1, 2, true));
    }
    
    [TestMethod]
    public void GetDevices_ShouldReturnListOfDevices()
    {
        var devices = new List<AHomeDevice> { new HomeDevice() };
        
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Devices).Returns(devices);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember>
        {
            new HomeMember { UserId = 1 , ListDevices = true}
        });
        _home.Setup(hm => hm.Owner).Returns(_user.Object);
        
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUser(It.IsAny<int>())).Returns(_user.Object);
        
        _user.Setup(x => x.UserId).Returns(1);

        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUser(It.IsAny<int>())).Returns(_user.Object);

        
        var result = _homeMemberService.GetDevices(1, 1,null);
        
        Assert.AreEqual(devices, result);
    }
    
    [TestMethod]
    public void CanListDevices_ShouldThrowExceptionWhenUserCannotListDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1, ListDevices = false }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = new HomeMember { UserId = 1, ListDevices = false };
    
        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.CanListDevices(homeMember));
    }
    
    [TestMethod]
    public void CanAddDevices_ShouldThrowExceptionWhenUserCannotAddDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1, AddDevices = false }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = new HomeMember { UserId = 1, AddDevices = false };
    
        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.CanAddDevices(homeMember));
    }
    
    [TestMethod]
    public void IsNotifiable_ShouldThrowExceptionWhenUserIsNotNotifiable()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1, Notifiable = false }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = new HomeMember { UserId = 1, Notifiable = false };
    
        Assert.ThrowsException<ConflictException>(() => _homeMemberService.IsNotifiable(homeMember));
    }
    
    [TestMethod]
    public void GetHomeMembers_ShouldReturnListOfHomeMembers()
    {
        var members = new List<AHomeMember> { new HomeMember() };
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(members);
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        
        var result = _homeMemberService.GetHomeMembers(1, 1);
        
        Assert.AreEqual(members, result);
    }

    [TestMethod]
    public void GetHomeMembers_ShouldThrowExceptionWhenUserIsNotOwner()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Owner.UserId).Returns(1);
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        
        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.GetHomeMembers(1, 2));
    }


    [TestMethod]
    public void GetHomeMemberWithHomeIdAndUserId_WhenCalled_ShouldReturnHomeMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1 }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var result = _homeMemberService.GetHomeMember(1, 1);
    
        Assert.AreEqual(1, result.UserId);
    }
    
    [TestMethod]
    public void CanUpdateDevices_WhenCalled_ShouldReturnTrue()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1, UpdateDevices = true }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = new HomeMember { UserId = 1, UpdateDevices = true };
    
        _homeMemberService.CanUpdateDevices(homeMember);
    }

    [TestMethod]
    public void CanUpdateDevices_WhenCalled_ShouldThrowException()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1, UpdateDevices = false }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = new HomeMember { UserId = 1, UpdateDevices = false };
    
        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.CanUpdateDevices(homeMember));
    }
    
    [TestMethod]
    public void SetUpdateDevices_WhenCalled_ShouldSetUpdateDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> 
        { 
            new HomeMember { UserId = 1 }
        });
    
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        _homeMemberService.SetUpdateDevices(1, 1, true);
    
        Assert.IsTrue(_home.Object.Members[0].UpdateDevices);
    }
    
    [TestMethod]
    public void SetUpdateDevices_ShouldSetUpdateDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        _homeMemberService.SetUpdateDevices(1, 1, true);
    
        Assert.IsTrue(_home.Object.Members[0].UpdateDevices);
    }
    
    [TestMethod]
    public void SetUpdateDevices_ShouldThrowExceptionWhenUserIsNotMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUserByEmail(It.IsAny<string>())).Returns(_user.Object);
        _user.Setup(x => x.UserId).Returns(1);

        Assert.ThrowsException<NotFoundException>(() => _homeMemberService.SetUpdateDevices(1, 2, true));
    }
    
    [TestMethod]
    public void GetHomeMember_ShouldReturnHomeMember()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        var homeMember = _homeMemberService.GetHomeMember(1, 1);
    
        Assert.AreEqual(1, homeMember.UserId);
    }
    
    [TestMethod]
    public void CanUpdateDevices_ShouldThrowExceptionWhenUserCannotUpdate()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1, UpdateDevices = false } });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
    
        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.CanUpdateDevices(new HomeMember { UserId = 1, UpdateDevices = false }));
    }

    [TestMethod]
    public void GetDevices_ShouldReturnDevicesIfUserIsOwner()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Devices).Returns(new List<AHomeDevice> { new HomeDevice() });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _homeServices.Setup(x => x.IsOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        var devices = _homeMemberService.GetDevices(1, 1, null);
    
        Assert.AreEqual(1, devices.Count);
    }
    
    [TestMethod]
    public void Validate_ShouldThrowExceptionWhenHomeIsFull()
    {
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember() });
        _home.Setup(hm => hm.MaxMembers).Returns(0);

        var methodInfo = typeof(HomeMemberService).GetMethod("Validate", BindingFlags.NonPublic | BindingFlags.Instance);
    
        try
        {
            methodInfo.Invoke(_homeMemberService, new object[] { _home.Object, _user.Object });
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (ConflictException ex)
        {
            var innerException = ex.InnerException;
            Assert.IsNotNull(innerException);
            Assert.IsInstanceOfType(innerException, typeof(ConflictException));
        }
    }

    [TestMethod]
    public void Validate_ShouldThrowExceptionWhenUserIsAlreadyMember()
    {
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember { UserId = 1 } });
        _user.Setup(x => x.UserId).Returns(1);

        var methodInfo = typeof(HomeMemberService).GetMethod("Validate", BindingFlags.NonPublic | BindingFlags.Instance);

        try
        {
            methodInfo.Invoke(_homeMemberService, new object[] { _home.Object, _user.Object });
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (TargetInvocationException ex)
        {
            var innerException = ex.InnerException;
            Assert.IsNotNull(innerException);
            Assert.IsInstanceOfType(innerException, typeof(ConflictException));
        }
    }
    
    [TestMethod]
    public void GetHomeMembers_ShouldThrowExceptionIfUserIsNotOwner()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Members).Returns(new List<AHomeMember> { new HomeMember() });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _userService.Setup(x => x.GetHomeUser(It.IsAny<int>())).Returns(_user.Object);

        Assert.ThrowsException<ForbiddenException>(() => _homeMemberService.GetHomeMembers(1, 2));
    }
    
    [TestMethod]
    public void GetDevices_ShouldReturnDevicesForOwner()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Devices).Returns(new List<AHomeDevice> { new HomeDevice() });
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _homeServices.Setup(x => x.IsOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        var devices = _homeMemberService.GetDevices(1, 1, null);
    
        Assert.AreEqual(1, devices.Count);
    }

    [TestMethod]
    public void GetDevices_ShouldThrowExceptionWhenUserCannotListDevices()
    {
        _home.Setup(hm => hm.Id).Returns(1);
        _home.Setup(hm => hm.Devices).Returns(new List<AHomeDevice>());
        _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(_home.Object);
        _homeServices.Setup(x => x.IsOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
        
        var expected = _homeMemberService.GetDevices(1, 1, null);
        
        Assert.AreEqual(expected.Count, 0);
    }

}