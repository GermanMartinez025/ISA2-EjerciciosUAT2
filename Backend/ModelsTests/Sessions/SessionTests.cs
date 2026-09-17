using ModelInterface.Users;
using Models.Sessions;
using Models.Users;
using Moq;

namespace ModelsTests.Sessions;

[TestClass]
public class SessionTests
{
    [TestMethod]
    public void NoParametrizedSessionConstructor_WhenCalled_CreatesNewSession()
    {
        var session = new Session() { Id = 1 };
        
        var result = session;
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Session));
        Assert.AreEqual(1, result.Id);
    }
    
    [TestMethod]
    public void ParametrizedSessionConstructor_WhenCalled_CreatesNewSession()
    {
        var user = new Mock<User>();
        user.Setup(u => u.Id).Returns(1);
        var session = new Session(user.Object);
        
        var result = session;
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Session));
        Assert.AreEqual(1, result.UserId);
        Assert.IsNotNull(result.Token);
        Assert.IsTrue(result.CreatedAt < DateTime.Now);
    }
    
    [TestMethod]
    public void ParametrizedSessionConstructor_WhenCalled_CreatesNewSessionWithUser()
    {
        var user = new Mock<User>();
        user.Setup(u => u.Id).Returns(1);
        var session = new Session(user.Object);
        
        var result = session;
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(Session));
        Assert.AreEqual(1, result.User.Id);
    }
}