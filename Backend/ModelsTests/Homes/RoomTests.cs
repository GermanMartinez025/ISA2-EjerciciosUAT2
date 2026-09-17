using ModelException;
using ModelInterface.Homes;
using Models.Homes;
using Moq;

namespace ModelsTests.Homes;

[TestClass]
public class RoomTests
{
    
    [TestMethod]
    public void ParametrizedRoomConstructor_WhenCalled_CreatesNewRoom()
    {
        Mock<AHome> home = new Mock<AHome>();
        var name = "Room";
        
        var room = new Room(home.Object, name);
        
        Assert.IsNotNull(room);
    }
    
    [TestMethod]
    public void ParametrizedRoom_WhenNameIsEmpty_ThrowsException()
    {
        Mock<AHome> home = new Mock<AHome>();
        var name = "";  
        
        Assert.ThrowsException<BadRequestException>(() => new Room(home.Object, name));
    }
    
    [TestMethod]
    public void ParametrizedRoom_WhenHomeIsNull_ThrowsException()
    {
        AHome homeObject = null;
        var name = "Room";
        
        Assert.ThrowsException<BadRequestException>(() => new Room(homeObject, name));
    }
    
    [TestMethod]
    public void ContructorEmptyRoom_WhenCalled_CreatesNewRoom()
    {
        var room = new Room();
        
        Assert.IsNotNull(room);
    }
}