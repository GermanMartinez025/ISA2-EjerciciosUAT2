using ModelInterface.Homes;
using Models.Homes;

namespace ServicesInterfaces.Homes;

public interface IRoomServices
{
    public Room CreateRoom (AHome home, string name);
    public ARoom AddRoomToHome(int homeId, string name);
    public ARoom GetRoom(int id);
    public List<ARoom> GetAllRooms();
    public ARoom DeleteRoom(int id);
    
}