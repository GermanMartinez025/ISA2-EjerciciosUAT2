using ControllersInterfaces.Sessions;
using ModelInterface.Users;
using ModelsAPI.Sessions;
using Moq;
using ServicesInterfaces.Sessions;
using WebAPI.APIs;

namespace WebAPITests.Sessions;

[TestClass]
public class SessionsAPITests
{
    private Mock<ISessionController> _sessionsController;
    private SessionAPI _sessionAPI;
    [TestInitialize]
    public void Initialize()
    {
        _sessionsController = new Mock<ISessionController>();
        _sessionAPI = new SessionAPI(_sessionsController.Object);
    }

    [TestMethod]
    public void CreateSessionTest()
    {
        var email = "admin@example.com";
        var password = "password123@";
        var request = new RequestCreateSession
        {
            Email = email,
            Password = password
        };

        _sessionsController.Setup(x => x.CreateSession(request));
        
        _sessionAPI.CreateSession(request);
    }
    
    [TestMethod]
    public void DeleteSessionTest()
    {
        var token = Guid.NewGuid().ToString();
        
        _sessionsController.Setup(x => x.DeleteSession(token));
        
        _sessionAPI.DeleteSession();
    }
}