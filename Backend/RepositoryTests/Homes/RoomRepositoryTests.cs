using Microsoft.EntityFrameworkCore;
using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Homes;
using Moq;
using Repositories.Homes;

namespace RepositoryTests.Homes;

[TestClass]
public class RoomRepositoryTests
{
    
    private Room _room;
    private AHome _home;
    private IQueryable<Room> _data;
    private Mock<DbSet<Room>> _mockSet;
    private Mock<DbContext> _dbContext;
    private RoomRepository _roomRepository;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _home = new Home("Main Street", 123, "casita", 82.123, -123.123, 3, new Mock<AHomeUser>().Object) { Id = 1 };
        _room = new Room(_home, "Room") { Id = 1 };

        _data = new List<Room>
        {
            _room
        }.AsQueryable();

        _mockSet = new Mock<DbSet<Room>>();
        _mockSet.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<Room>()).Returns(_mockSet.Object);
        _roomRepository = new RoomRepository(_dbContext.Object);
    }
    
    [TestMethod]
    public void CreateRoom_ShouldAddRoom()
    {
        _roomRepository.Create(_room);
        
        _mockSet.Verify(m => m.Add(It.IsAny<Room>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    [TestMethod]
    public void GetById_ShouldReturnRooms()
    {
        var result = _roomRepository.GetById(1);
        
        Assert.AreEqual(_room, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllRooms()
    {
        var result = _roomRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
    
    [TestMethod]
    public void DeleteRoom_ShouldRemoveRoom()
    {
        _roomRepository.Delete(_room);
        
        _mockSet.Verify(m => m.Remove(It.IsAny<Room>()), Times.Once());
        _dbContext.Verify(m => m.SaveChanges(), Times.Once());
    }
    
    
}