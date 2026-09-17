using IRepositories.Repositories.HomesRepositories;
using ModelException;
using ModelInterface.Homes;
using Models.Homes;
using ServicesInterfaces.Homes;

namespace Services.Homes;

public class RoomServices : IRoomServices
{
    private readonly IRoomRepository _roomRepository;
    private readonly IHomeServices _homeServices;
    
    public RoomServices(IRoomRepository roomRepository, IHomeServices homeServices)
    {
        _roomRepository = roomRepository;
        _homeServices = homeServices;
    }
    
    
    public Room CreateRoom (AHome home, string name)
    {
        Room room = new Room(home, name);
        return room;
    }
    
    public ARoom AddRoomToHome(int homeId, string name)
    {
        AHome home = _homeServices.GetHome(homeId);
        
        ValidateHomeNotFound(home);
        
        ARoom room = CreateRoom(home, name);
        
        _homeServices.AddRoomToHome(home, room);        
        _roomRepository.Create(room);
        
        return room;
    }

    public ARoom GetRoom(int id)
    {
        ARoom room = _roomRepository.GetById(id);
        
        ValidateRoomNotFound(room);

        return room;
    }

    public List<ARoom> GetAllRooms()
    {
        return _roomRepository.GetAll();
    }

    public ARoom DeleteRoom(int id)
    {
        ARoom room = GetRoom(id);
        _roomRepository.Delete(room);
        return room;
    }
    
    private void ValidateHomeNotFound(AHome home)
    {
        if (home == null)
        {
            throw new BadRequestException("Home not found.");
        }
    }

    private void ValidateRoomNotFound(ARoom room)
    {
        if (room == null)
        {
            throw new BadRequestException("Room not found.");
        }
    }
}