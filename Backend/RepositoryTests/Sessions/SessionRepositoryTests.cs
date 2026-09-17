using Microsoft.EntityFrameworkCore;
using ModelInterface.Sessions;
using ModelInterface.Users;
using Models.Sessions;
using Models.Users;
using Moq;
using Repositories.SessionRepositories;

namespace RepositoryTests.Sessions;

[TestClass]
public class SessionRepositoryTests
{
    private Session _session;
    private Mock<User> _user;
    private Mock<DbContext> _dbContext;
    private IQueryable<Session> _data;
    private Mock<DbSet<Session>> _mockSet;
    private SessionRepository _sessionRepository;
    private string _guidSession;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _user = new Mock<User>();
        _user.Setup(u => u.Id).Returns(1);
        _session = new Session(_user.Object) { Id = 1 };
        _guidSession = _session.Token.ToString();
        _data = new List<Session>
        {
            _session
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<Session>>();
        
        _mockSet.As<IQueryable<Session>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Session>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Session>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Session>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<Session>()).Returns(_mockSet.Object);
        _sessionRepository = new SessionRepository(_dbContext.Object);
    }
    
    [TestMethod]
    public void CreateSession_ShouldAddNewSession()
    {
        _sessionRepository.Create(_session);
        
        _mockSet.Verify(m => m.Add(It.IsAny<Session>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());   
    }
    
    [TestMethod]
    public void GetById_ShouldReturnSession()
    {
        var session = _sessionRepository.GetById(1);
        
        Assert.AreEqual(_session, session);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllSessions()
    {
        var sessions = _sessionRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), sessions.Count);
    }
    
    [TestMethod]
    public void DeleteSession_ShouldRemoveSession()
    {
        _sessionRepository.Delete(_session);
        
        _mockSet.Verify(m => m.Remove(It.IsAny<Session>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void GetById_ShouldThrowExceptionWhenSessionNotFound()
    {
        Assert.IsNull(_sessionRepository.GetById(2));
    }
    
    [TestMethod]
    public void GetByToken_ShouldReturnSession()
    {
        var session = _sessionRepository.GetByToken(_guidSession);
        
        Assert.AreEqual(_session, session);
    }
    
    [TestMethod]
    public void GetByToken_ShouldThrowExceptionWhenSessionNotFound()
    {
        Assert.IsNull(_sessionRepository.GetByToken("token2"));
    }
}