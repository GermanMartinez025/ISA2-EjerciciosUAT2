using ControllersInterfaces.Homes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI;
using ModelsAPI.Devices;
using ModelsAPI.Homes;
using ModelsAPI.Users;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/homes")]
[ExceptionFilter]
public class HomeAPI : ControllerBase
{
    private readonly IHomeController _homeController;

    public HomeAPI(IHomeController homeController)
    {
        _homeController = homeController;
        
    }
    
    [HttpPost]
    public GenericResponse CreateHome([FromBody] RequestCreateHome request)
    {
        var token = Request.Headers["Authorization"];
        ResponseCreateHome response = _homeController.CreateHome(request, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Home created successfully";
        return genericResponse;
    }

    [HttpPost("{homeId}/members")]
    public GenericResponse AddUserToHome(int homeId, [FromBody] RequestAddUserToHome request)
    {
        ResponseAddUserToHome response = _homeController.AddUserToHome(homeId, request);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "User added to home successfully";
        return genericResponse;
    }
    
    [HttpPut("{homeId}/members/{userId}")]
    public GenericResponse UpdateHomeMember(int homeId, int userId, [FromBody] RequestUpdateHomeMember request)
    {
        
            ResponseUpdateHomeMember response = _homeController.UpdateHomeMember(homeId, userId, request);
            _homeController.UpdateHomeMember(homeId, userId, request);
            var genericResponse = new GenericResponse();
            genericResponse.Data = response;
            genericResponse.ExecutionSuccessful = true;
            genericResponse.Message = "User updated successfully";
            return genericResponse;
    }
    
    [HttpPut]
    [Route("{homeId}/devices/{hardwareId}")]
    public GenericResponse UpdateDevice([FromBody]RequestUpdateHomeDevice request)
    {
        var token = Request.Headers["Authorization"];
        var homeId = int.Parse(Request.RouteValues["homeId"].ToString());
        var hardwareId = Guid.Parse(Request.RouteValues["hardwareId"].ToString());
            
        var response = _homeController.UpdateDevice(homeId, hardwareId, request, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Device updated successfully";
        return genericResponse;
    }
    
    [HttpGet("{homeId}/devices")]
    public GenericResponse GetDevicesFilter(string? roomName)
    {
        var token = Request.Headers["Authorization"];
        var homeId = int.Parse(Request.RouteValues["homeId"].ToString());
        
        ResponseGetDevices response = _homeController.GetDevices(homeId, token, roomName);
        
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Devices retrieved successfully";
        return genericResponse;
    }
    
    [HttpGet("{homeId}/members")]
    public GenericResponse GetHomeMembers()
    {
        var token = Request.Headers["Authorization"];
        var homeId = int.Parse(Request.RouteValues["homeId"].ToString());
        
        ResponseGetMembers response = _homeController.GetMembers(homeId, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Members retrieved successfully";
        return genericResponse;
    }
    
    [HttpPost("{homeId}/devices")]
    public GenericResponse CreateDevice(int homeId, [FromBody] int deviceId)
    {
        var token = Request.Headers["Authorization"];
        ResponseHomeDevice response = _homeController.CreateDevice(homeId, deviceId, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Device created successfully";
        return genericResponse;
    }
    
    [HttpPut("{homeId}")]
    [AuthorizationFilter("HomeUser")]
    public GenericResponse UpdateName(int homeId, [FromBody] RequestUpDateHome request)
    {
        var token = Request.Headers["Authorization"];
        ResponseGetHome response = _homeController.UpDateName(homeId, request, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Home name updated successfully";
        return genericResponse;
    }
    
    [HttpPost ("{homeId}/rooms")]
    [AuthorizationFilter("HomeUser")]
    public GenericResponse CreateRoom(int homeId, [FromBody] RequestCreateRoom request)
    {
        var token = Request.Headers["Authorization"];
        ResponseCreateRoom response = _homeController.CreateRoom(homeId, request, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Room created successfully";
        return genericResponse;
    }
    
    [HttpPost("{homeId}/rooms/{roomId}/devices")]
    [AuthorizationFilter("HomeUser")]
    public GenericResponse AssignDeviceToRoom(int homeId, int roomId, [FromBody] Guid hardwareId)
    {
        var token = Request.Headers["Authorization"];
        var response = _homeController.AssignDeviceToRoom(homeId, hardwareId, roomId, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Device assigned to room successfully";
        return genericResponse;
      
    }
    
    [HttpGet]
    [AuthorizationFilter("HomeUser")]
    public GenericResponse GetHomes()
    {
        var token = Request.Headers["Authorization"];
        ResponseGetHomes response = _homeController.GetHomes(token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Homes retrieved successfully";
        return genericResponse;
    }
    
}