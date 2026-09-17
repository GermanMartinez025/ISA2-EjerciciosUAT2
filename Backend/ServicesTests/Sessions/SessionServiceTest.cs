using IRepositories.Repositories.SessionRepositories;
using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Sessions;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users;
using Moq;
using Services.Sessions;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace ServicesTests.Sessions;

[TestClass]
public class SessionServiceTest
{
    private Mock<ISessionRepository> _sessionRepository;
    private Mock<IUserService> _userService;
    private ISessionService _sessionService;
    private Mock<User> _user;
    private Mock<AUserType> _userRole;
    private List<AUserType> _userRoles;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepository = new Mock<ISessionRepository>();
        _userService = new Mock<IUserService>();
        _sessionService = new SessionService(_sessionRepository.Object, _userService.Object);
        _user = new Mock<User>();
        _userRole = new Mock<AUserType>();
        _userRole.Setup(x => x.Role).Returns(RolesEnum.Admin);
        _userRoles = new List<AUserType>();
        _userRoles.Add(_userRole.Object);
        _user.Setup(x => x.Roles).Returns(_userRoles);
    }

    [TestMethod]
    public void LoginTest()
    {
        var email = "admin@admin.com";
        var password = "password123@";
        _userService.Setup(x => x.ValidateCredentials(email, password));
        _userService.Setup(x => x.GetByEmail(email)).Returns(_user.Object);

        var result = _sessionService.Login(email, password);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ISession));
    }

    [TestMethod]
    public void GetUserFromSessionTest()
    {
        var token = Guid.NewGuid().ToString();
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);
        _userService.Setup(x => x.GetById(session.Object.UserId)).Returns(_user.Object);

        var result = _sessionService.GetUserFromSession(token);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(User));
    }
    
    [TestMethod]
    public void GetUserIdFromSessionTest()
    {
        var token = Guid.NewGuid().ToString();
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);
        _userService.Setup(x => x.GetById(session.Object.UserId)).Returns(_user.Object);

        var result = _sessionService.GetUserIdFromSession(token);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(int));
    }
    
    [TestMethod]
    public void ValidSessionTest()
    {
        var token = Guid.NewGuid().ToString();
        var role = "Admin";
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);
        _userService.Setup(x => x.GetById(session.Object.UserId)).Returns(_user.Object);
        _user.Setup(x => x.Roles).Returns(_userRoles);
        _user.Setup(x => x.HasRole(role)).Returns(true);

        var result = _sessionService.ValidSession(token, role);

        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void LogoutTest()
    {
        var token = Guid.NewGuid().ToString();
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);

        _sessionService.Logout(token);

        _sessionRepository.Verify(x => x.Delete(session.Object), Times.Once);
    }
    
    [TestMethod]
    public void GetRoleFromSessionTest()
    {
        var token = Guid.NewGuid().ToString();
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);
        _userService.Setup(x => x.GetById(session.Object.UserId)).Returns(_user.Object);
        _user.Setup(x => x.Roles).Returns(_userRoles);

        var result = _sessionService.GetRolesFromSession(token)[0];

        Assert.AreEqual("Admin", result);
    }
    
    [TestMethod]
    public void GetRoleFromSessionTest_NoSession()
    {
        var token = Guid.NewGuid().ToString();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns((ISession)null);

        Assert.ThrowsException<NotFoundException>(() => _sessionService.GetRolesFromSession(token)[0]); 
    }
    
    [TestMethod]
    public void ValidSession_ListHasMatchingRole_ReturnsTrue()
    {
        var token = Guid.NewGuid().ToString();
        var roles = new List<string> { "Admin", "User" };
        var session = new Mock<ISession>();
        _sessionRepository.Setup(x => x.GetByToken(token)).Returns(session.Object);
        _userService.Setup(x => x.GetById(session.Object.UserId)).Returns(_user.Object);
        _user.Setup(x => x.HasRole("Admin")).Returns(true);
        
        var result = _sessionService.ValidSession(token, roles);
        
        Assert.IsTrue(result);
    }
}