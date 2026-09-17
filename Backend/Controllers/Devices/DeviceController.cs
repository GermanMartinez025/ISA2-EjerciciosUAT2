using ControllersInterfaces.Devices;
using ModelInterface.Devices;
using ModelsAPI.Devices;
using Services.Devices;
using Mapper;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace Controllers.Devices;

public class DeviceController : IDeviceController
{
    private IDeviceServices _deviceService;
    private ICompanyOwnerService _companyOwnerService;
    private readonly ISessionService _sessionService;
        
    public DeviceController(IDeviceServices deviceService, ICompanyOwnerService companyOwnerService ,ISessionService sessionService)
    {
        _deviceService = deviceService;
        _companyOwnerService = companyOwnerService;
        _sessionService = sessionService;
    }

    public ResponseCreateDevice CreateDevice(RequestCreateDevice request, string token)
    {
        var user = _sessionService.GetUserFromSession(token);
        
        var companyOwner = _companyOwnerService.GetById(user.Id);
        
        _companyOwnerService.ValidateAssignedCompany(companyOwner);
        
        var device = _deviceService.AddDevice(request, companyOwner.Company);
        
        return new ResponseCreateDevice(device);
    }
    
    public ResponseGetAllDevices GetFilterDevices(int page, int pageSize, List<string>? deviceName, List<string>? deviceModel, List<string>? companyName, List<string>? deviceType)
    {
        Dictionary<string, object> filtersDictionary = new Dictionary<string, object>();
        if (deviceName != null) filtersDictionary.Add("deviceName", deviceName);
        if (deviceModel != null) filtersDictionary.Add("deviceModel", deviceModel);
        if (companyName != null) filtersDictionary.Add("companyName", companyName);
        if (deviceType != null) filtersDictionary.Add("deviceType", deviceType);
        
        var devices = _deviceService.GetFilterDevices(page, pageSize, filtersDictionary);
        
        var responseDevices = MapDevices(devices);   
        responseDevices.TotalDevices = _deviceService.GetAmountOfDevices(filtersDictionary);
        responseDevices.actualPage = page;
        responseDevices.totalPages = (int)Math.Ceiling((double)responseDevices.TotalDevices / pageSize);
       
        return responseDevices;
    }

    private ResponseGetAllDevices MapDevices(List<IDevice> devices)
    {
        var responseDevices = new ResponseGetAllDevices();
        var mapperDevice = new Mapper<IDevice, ResponseDevice>();
        
        foreach (var device in devices)
        {
            responseDevices.Devices.Add(mapperDevice.Convert(device));
        }
        
        return responseDevices;
    }
    
    public ResponseGetTypeDevice GetSupportedDevices()
    {
        var deviceType = IDeviceServices.GetSupportedDevices();
        
        var responseDeviceTypes = new ResponseGetTypeDevice();
        responseDeviceTypes.DeviceTypes = deviceType;
        
        return responseDeviceTypes;
    }
    
}