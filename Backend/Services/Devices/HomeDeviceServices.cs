using IRepositories.Repositories.DeviceRepositories;
using ModelException;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using Models.Devices;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Users;

namespace Services.Devices;

public class HomeDeviceServices : IHomeDeviceServices
{
    private readonly IHomeServices _homeServices;
    private readonly IDeviceServices _deviceServices;
    private readonly IHomeDeviceRepository _homeDeviceRepository;
    private readonly IHomeMemberService _homeMemberService;
    private readonly IRoomServices _roomServices;
    
    public HomeDeviceServices(IHomeDeviceRepository  homeDeviceRepository,IHomeServices homeServices, IDeviceServices deviceServices, 
        IHomeMemberService homeMemberService, IRoomServices roomServices)
    {
        _homeDeviceRepository = homeDeviceRepository;
        _homeServices = homeServices;
        _deviceServices = deviceServices;
        _homeMemberService = homeMemberService;
        _roomServices = roomServices;
    }
    public AHomeDevice Create(ADevice device, AHome home) 
    {
        HomeDevice homeDevice = new HomeDevice(device, home);
        
        return homeDevice;
    }

    public AHomeDevice AddHomeDevice(int homeId, int deviceId, int userId)
    {
        var home = _homeServices.GetHome(homeId);
        var device = _deviceServices.GetDevice(deviceId);
        
        ValidateUserCanAddDevice(homeId, userId);
        
        var homeDevice = Create(device, home);
        
        _homeDeviceRepository.Create((HomeDevice)homeDevice);
        
        return homeDevice;
    }
    
    public AHomeDevice GetDevice(Guid deviceId)
    {
        AHomeDevice homeDevice = _homeDeviceRepository.GetById(deviceId);

        ValidateDeviceIsNotNull(homeDevice);

        return homeDevice;
    }
    
    public void CreateEvent (Guid deviceId, EventType eventType)
    {
        var device = GetDevice(deviceId);
        device.GenerateEvent(eventType);
        _homeDeviceRepository.SaveChanges();
    }

    public AHomeDevice ChangeName(Guid deviceId, string newName)
    {
        var device = GetDevice(deviceId);
        device.Name = newName;
        _homeDeviceRepository.SaveChanges();
        return device;
    }
    
    public AHomeDevice AssignDeviceToRoom(int homeId, Guid deviceId, int roomId, int userId)
    {
        var room = _roomServices.GetRoom(roomId);

        ValidateUserIsOwner(homeId, userId);
        ValidateRoomBelongsToHome(homeId, room);

        var homeDevice = _homeDeviceRepository.GetById(deviceId);
        
        ValidateDeviceIsNotNull(homeDevice);
        
        room.Devices.Add(homeDevice);
        homeDevice.RoomId = roomId;
        homeDevice.Room = room;

        _homeDeviceRepository.SaveChanges();

        return homeDevice;
    }
    
    public List<AHomeDevice> ListDeviceInRoom(ARoom room)
    {
        return room.Devices;
    }
    private void ValidateUserCanAddDevice(int homeId, int userId)
    {
        var user = _homeMemberService.GetHomeMember(homeId, userId);
        _homeMemberService.CanAddDevices(user);
    }
    
    private void ValidateDeviceIsNotNull(AHomeDevice homeDevice)
    {
        if (homeDevice == null){
            throw new NotFoundException("HomeDevice not found");
        }
    }

    private void ValidateUserIsOwner(int homeId, int userId)
    {
        if (!_homeServices.IsOwner(homeId, userId))
        {
            throw new UnauthorizedAccessException("User is not the owner of the home.");
        }
    }

    private void ValidateRoomBelongsToHome(int homeId, ARoom room)
    {
        if (room.HomeId != homeId)
        {
            throw new BadRequestException("The room does not belong to the specified home.");
        }
    }
}
