using Controllers.Sessions;
using ModelInterface.Sessions;
using ModelInterface.Users;
using Models.Users;
using ModelsAPI.Sessions;
using Moq;
using ServicesInterfaces.Sessions;

namespace ControllerTests.Sessions;

[TestClass]
public class SessionControllerTests
{
    private Mock<ISessionService> _sessionService;
    private SessionController _sessionController;
    private Mock<User> _user;
    private Mock<ISession> _session;

    [TestInitialize]
    public void Initialize()
    {
        _sessionService = new Mock<ISessionService>();
        _sessionController = new SessionController(_sessionService.Object);
        _user = new Mock<User>();
        _session = new Mock<ISession>();
    }

    [TestMethod]
    public void LoginTest()
    {
        var email = "admin@example.com";
        var password = "password123@";
        var request = new RequestCreateSession
        {
            Email = email,
            Password = password
        };

        _sessionService.Setup(x => x.Login(email, password)).Returns(_session.Object);

        _sessionController.CreateSession(request);

        _sessionService.Verify(x => x.Login(email, password), Times.Once);
    }
    
    [TestMethod]
    public void LogoutTest()
    {
        var token = Guid.NewGuid().ToString();
        
        _sessionController.DeleteSession(token);
        
        _sessionService.Verify(x => x.Logout(token), Times.Once);
    }

}