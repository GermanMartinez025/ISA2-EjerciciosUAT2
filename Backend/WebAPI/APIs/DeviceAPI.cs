using ControllersInterfaces.Devices;
using ControllersInterfaces.Validations;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI;
using ModelsAPI.Devices;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/devices")]
[ExceptionFilter]
public class DeviceAPI : ControllerBase
{
    private readonly IDeviceController _deviceController;
    private readonly IHomeDeviceController _homeDeviceController;
    private readonly IValidationController _validationController;
    
    public DeviceAPI(IDeviceController deviceController, IHomeDeviceController homeDeviceController, IValidationController validationController)
    {
        _deviceController = deviceController;
        _homeDeviceController = homeDeviceController;
        _validationController = validationController;
    }
    
    [HttpPost]
    [AuthorizationFilter("CompanyOwner")]
    public GenericResponse CreateDevice([FromBody]RequestCreateDevice request)
    {
        string token = Request.Headers["Authorization"];
        var response = _deviceController.CreateDevice(request, token); 
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Device added successfully";
        return genericResponse;
    }
    
    [HttpPost]
    [Route("events")]
    public void SendEvent([FromBody]RequestEvent request)
    {
        string hardwareId = Request.Headers["Authorization"] ;
        _homeDeviceController.SendEvent(hardwareId, request);
    }
    
    [HttpGet]
    public GenericResponse GetDevicesFilter(int page, int pageSize, [FromQuery] List<string>? deviceName, [FromQuery] List<string>? deviceModel, [FromQuery] List<string>? companyName,[FromQuery] List<string>? deviceType)
    {
        var response = _deviceController.GetFilterDevices(page, pageSize, deviceName, deviceModel, companyName, deviceType);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Devices retrieved successfully";
        return genericResponse;
    }
    
    [HttpGet]
    [Route("supported")]
    public GenericResponse GetSupportedDevices()
    {
        var response = _deviceController.GetSupportedDevices();
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Device types retrieved successfully";
        return genericResponse;
    }
    
    [HttpGet]
    [Route("validations")]
    public GenericResponse GetValidators()
    {
        var response = _validationController.GetAllValidators();
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Validators retrieved successfully";
        return genericResponse;
    }
    
}