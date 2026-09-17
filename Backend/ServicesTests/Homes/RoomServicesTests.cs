using IRepositories.Repositories.HomesRepositories;
using ModelException;
using ModelInterface.Homes;
using Moq;
using Services.Homes;
using ServicesInterfaces.Homes;

namespace ServicesTests.Homes
{
    [TestClass]
    public class RoomServicesTests
    {
        private Mock<IRoomRepository> _roomRepository;
        private Mock<IHomeServices> _homeServices;
        private IRoomServices _roomServices;
        private ARoom _room;

        [TestInitialize]
        public void TestInitialize()
        {
            _roomRepository = new Mock<IRoomRepository>();
            _homeServices = new Mock<IHomeServices>();
            _homeServices.Setup(x => x.GetHome(It.IsAny<int>())).Returns(new Mock<AHome>().Object);
            _roomServices = new RoomServices(_roomRepository.Object, _homeServices.Object);
            _room = new Mock<ARoom>().Object; 
        }

        [TestMethod]
        public void AddRoomToHome_ShouldCallRepositoryCreateAndReturnRoom()
        {
            int homeId = 1;
            string name = "room";
            var home = new Mock<AHome>().Object;
            _roomRepository.Setup(x => x.Create(It.IsAny<ARoom>())).Returns(_room);

            var result = _roomServices.AddRoomToHome(homeId, name);

            _roomRepository.Verify(x => x.Create(It.IsAny<ARoom>()), Times.Once);
        }
        
        [TestMethod]
        public void AddRoomToHome_ShouldThrowExceptionWhenHomeNotFound()
        {
            int homeId = 1;
            string name = "room";
            _homeServices.Setup(x => x.GetHome(homeId)).Returns((AHome)null);

            Assert.ThrowsException<BadRequestException>(() => _roomServices.AddRoomToHome(homeId, name));
        }

        [TestMethod]
        public void GetRoom_ShouldCallRepositoryGetByIdAndReturnRoom()
        {
            int roomId = 1;
            _roomRepository.Setup(x => x.GetById(roomId)).Returns(_room);

            var result = _roomServices.GetRoom(roomId);

            _roomRepository.Verify(x => x.GetById(roomId), Times.Once);
            Assert.AreEqual(_room, result);
        }

        [TestMethod]
        public void GetAllRooms_ShouldCallRepositoryGetAllAndReturnListOfRooms()
        {
            var rooms = new List<ARoom> { _room, _room };
            _roomRepository.Setup(x => x.GetAll()).Returns(rooms);

            var result = _roomServices.GetAllRooms();

            _roomRepository.Verify(x => x.GetAll(), Times.Once);
            CollectionAssert.AreEqual(rooms, result);
        }

        [TestMethod]
        public void DeleteRoom_ShouldCallRepositoryDeleteAndReturnDeletedRoom()
        {
            int roomId = 1;
            _roomRepository.Setup(x => x.GetById(roomId)).Returns(_room);

            var result = _roomServices.DeleteRoom(roomId);

            _roomRepository.Verify(x => x.GetById(roomId), Times.Once);
            _roomRepository.Verify(x => x.Delete(_room), Times.Once);
            Assert.AreEqual(_room, result);
        }
        
        [TestMethod]
        public void CreateRoom_ShouldReturnRoom()
        {
            var home = new Mock<AHome>().Object;
            string name = "room";
            
            var result = _roomServices.CreateRoom(home, name);
            
            Assert.IsNotNull(result);
        }
    }
}
