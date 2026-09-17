using ControllersInterfaces.Users;
using Microsoft.AspNetCore.Mvc;
using ModelsAPI;
using ModelsAPI.Users;
using WebAPI.Filters;

namespace WebAPI.APIs;

[ApiController]
[Route("api/users")]
[ExceptionFilter]
public class UserAPI : ControllerBase
{
    private readonly IUserController _userController;
    public UserAPI(IUserController userController)
    {
        _userController = userController;
    }
    
    [HttpPost]
    public GenericResponse AddUser(RequestAddUser requestAddUser)
    {
        var token = Request.Headers["Authorization"];
        var finalToken = token == "null" ? null : token.ToString();
        var response = _userController.AddUser(requestAddUser, finalToken);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "User added successfully";
        return genericResponse;
    }
    
    [AuthorizationFilter("Admin")]
    [HttpGet]
    public GenericResponse GetUsers(int page, int pageSize, [FromQuery] List<string>? roles, [FromQuery] List<string>? names)
    {
        var response = _userController.GetUsers(page, pageSize, roles, names);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "Users retrieved successfully";
        return genericResponse;
    }
    
    [AuthorizationFilter("Admin")]
    [HttpDelete("{userId}")]
    public GenericResponse DeleteUser()
    {
        var userId = int.Parse(RouteData.Values["userId"].ToString());
        var token = Request.Headers["Authorization"];
        var response = _userController.DeleteUser(userId, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "User deleted successfully";
        return genericResponse;
    }
    
    [AuthorizationFilter("Admin", "CompanyOwner")]
    [HttpPut]
    public GenericResponse UpdateUser(RequestUpdateUser requestUpdateUser)
    {
        var token = Request.Headers["Authorization"];
        var response = _userController.UpdateUser(requestUpdateUser, token);
        var genericResponse = new GenericResponse();
        genericResponse.Data = response;
        genericResponse.ExecutionSuccessful = true;
        genericResponse.Message = "User updated successfully";
        return genericResponse;
    }
    

}