using ControllersInterfaces.Homes;
using ModelInterface.Devices;
using ServicesInterfaces.Users;
using Mapper;
using ModelException;
using ModelInterface.Users;
using Models.Homes;
using ModelsAPI.Devices;
using ModelsAPI.Homes;
using ModelsAPI.Users;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Homes;
using ServicesInterfaces.Sessions;

namespace Controllers.Homes;

public class HomeController : IHomeController
{
    private IHomeServices _homeServices;
    private IHomeMemberService _homeMemberService;
    private ISessionService _sessionService;
    private IHomeDeviceServices _homeDeviceServices;
    private IRoomServices _roomServices;
    
    public HomeController(IHomeServices homeServices, IHomeMemberService homeMemberService, ISessionService sessionService, 
        IHomeDeviceServices homeDeviceServices, IRoomServices roomServices)
    {
        _homeServices = homeServices;
        _homeMemberService = homeMemberService;
        _sessionService = sessionService;
        _homeDeviceServices = homeDeviceServices;
        _roomServices = roomServices;
    }
    
    public ResponseAddUserToHome AddUserToHome(int homeId, RequestAddUserToHome request)
    {
        _homeMemberService.AddUserToHome(homeId, request.Email);
        return new ResponseAddUserToHome(homeId, request.Email);
    }
    public ResponseUpdateHomeMember UpdateHomeMember(int homeId, int userId, RequestUpdateHomeMember request)
    {
       
        var user = _homeMemberService.GetHomeMember(homeId, userId);
        
        if (request.Notifiable != null)
        {
            _homeMemberService.SetNotifiable(homeId, userId, request.Notifiable.Value);
        }

        if (request.ListDevices != null)
        { 
            _homeMemberService.SetListDevices(homeId, userId, request.ListDevices.Value);
        }

        if (request.AddDevices != null)
        {
            _homeMemberService.SetAddDevices(homeId, userId, request.AddDevices.Value);
        }

        return new ResponseUpdateHomeMember(homeId, userId, user.Notifiable, user.ListDevices, user.AddDevices);
    }
    public ResponseGetDevices GetDevices(int homeId, string token, string? rooomName)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.Id;
        List<AHomeDevice> devices = _homeMemberService.GetDevices(homeId, userId, rooomName);
        return MapHomeDevices(devices);
    }
    
    public ResponseCreateHome CreateHome(RequestCreateHome request, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.Id;
       
        var home = _homeServices.AddHome(request, userId);
        return new ResponseCreateHome(home);   
    }

    public ResponseHomeDevice CreateDevice(int homeId, int deviceId, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.HomeUser.Id;
        
        var device = _homeDeviceServices.AddHomeDevice(homeId, deviceId, userId);
        return new ResponseHomeDevice(device);
    }
    
    public ResponseCreateRoom CreateRoom(int homeId, RequestCreateRoom request, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.Id;
        var room = _roomServices.AddRoomToHome(homeId, request.Name);
        
        return new ResponseCreateRoom(room);
    }
    
    public ResponseGetHome UpDateName(int homeId, RequestUpDateHome request, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.Id;
        var home = _homeServices.GetHome(homeId);
        _homeMemberService.GetHomeMember(homeId, userId);
        home.Name = request.Name;
        var result =_homeServices.Update(home);
        
        return new ResponseGetHome(result);
    }
    private ResponseGetDevices MapHomeDevices(List<AHomeDevice> devices)
    {
        var responseDevices = new ResponseGetDevices();
        var mapperDevice = new Mapper<AHomeDevice, ResponseHomeDevice>();

        foreach (var device in devices)
        {
            responseDevices.Devices.Add(mapperDevice.Convert(device));
        }

        return responseDevices;
    }
    public ResponseGetMembers GetMembers(int homeId, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var userId = user.Id;
        List<AHomeMember> members = _homeMemberService.GetHomeMembers(homeId, userId);
        return MapHomeMembers(members);
    }
    private ResponseGetMembers MapHomeMembers(List<AHomeMember> members)
    {
        ResponseGetMembers responseMembers = new ResponseGetMembers();

        foreach (var member in members)
        {
            responseMembers.Members.Add(new ResponseGetHomeMember(member));
        }

        return responseMembers;
    }
    
    public ResponseHomeDevice UpdateDevice(int homeId, Guid hardwareId, RequestUpdateHomeDevice request, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        ValidateUserCanUpdateDevice(homeId, user.Id);
        var device = _homeDeviceServices.GetDevice(hardwareId);
        
        if (request.Name != null)
        {
            _homeDeviceServices.ChangeName(hardwareId, request.Name);
        }
        else
        {
            throw new BadRequestException("Name is required, please provide a name");
        }
        
        return new ResponseHomeDevice(device);
    }
    
    private void ValidateUserCanUpdateDevice(int homeId, int userId)
    {
        var user = _homeMemberService.GetHomeMember(homeId, userId);
        if (user == null)
        {
            throw new BadRequestException("User is not member of this home");
        }
        _homeMemberService.CanUpdateDevices(user);
    }
    
    public ResponseHomeDevice AssignDeviceToRoom(int homeId, Guid hardwareId, int roomId, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var device = _homeDeviceServices.AssignDeviceToRoom(homeId, hardwareId, roomId, user.Id);
        return new ResponseHomeDevice(device);
    }
    
    public ResponseGetHomes GetHomes(string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        var homes = _homeServices.GetMyHomes(user.Id);
        return new ResponseGetHomes(homes);
    }
    
}