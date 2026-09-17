using ControllersInterfaces.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelException;
using ModelInterface.Users;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI;
using ModelsAPI.Users;
using Moq;
using WebAPI.APIs;

namespace WebAPITests.Users;

[TestClass]
public class UserAPITests
{
    private Mock<IUserController> _userController;
    private UserAPI _userApi;

    [TestInitialize]
    public void Setup()
    {
        _userController = new Mock<IUserController>();
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "token";
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _userApi = new UserAPI(_userController.Object);
        _userApi.ControllerContext = controllerContext;
    }

    [TestMethod]
    public void AddHomeUser_WhenCalled_ReturnsOk()
    {
        var admin = new Admin();
        var request = new RequestAddUser() {Email = "john@example.com", Password = "password", FirstName = "John", LastName = "Doe", Role = "Admin"};
        _userController.Setup(x => x.AddUser(request, "token")).Returns(new ResponseGetUser());

        var result = _userApi.AddUser(request);

        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }

    [TestMethod]
    public void GetUsers_WhenCalled_ReturnsResponseGetUsers()
    {
        var users = new List<AUser>()
        {
            new Admin("Admin", "Admin", "admin@example.com", "password1@").User,
            new HomeUser("HomeUser", "HomeUser", "user@example.com", "password1@", "profile.png").User
        };

        _userController.Setup(x => x.GetUsers(1, 10, null, null)).Returns(new ResponseGetUsers());

        var result = _userApi.GetUsers(1, 10, null, null);
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }

    [TestMethod]
    public void GetUsersWithSpecificRoles_WhenCalled_ReturnsResponseGetUsers()
    {
        var users = new List<AUser>()
        {
            new Admin("Admin", "Admin", "admin@admin.com", "password1@").User,
            new HomeUser("HomeUser", "HomeUser", "user@example.com", "password1@", "profile.png").User
        };

        _userController.Setup(x => x.GetUsers(1, 10, new List<string>() { "Admin" }, null)).Returns(new ResponseGetUsers());

        var result = _userApi.GetUsers(1, 10, new List<string>() { "Admin" }, null);
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
    }
    
   
    [TestMethod]
    public void DeleteUser_WhenCalled_ReturnsGenericResponse()
    {
        int userId = 1;
        _userController.Setup(x => x.DeleteUser(userId, "token")).Returns(new ResponseDeleteUser());

        _userApi.ControllerContext.RouteData = new Microsoft.AspNetCore.Routing.RouteData();
        _userApi.ControllerContext.RouteData.Values["userId"] = userId.ToString();
        
        var result = _userApi.DeleteUser();
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("User deleted successfully", result.Message);
    }
    
    [TestMethod]
    public void UpdateUser_WhenCalled_ReturnsGenericResponse()
    {
        var request = new RequestUpdateUser() {Role = "Admin"};
        _userController.Setup(x => x.UpdateUser(request, "token")).Returns(new ResponseGetUser());

        var result = _userApi.UpdateUser(request);
        
        Assert.IsInstanceOfType(result, typeof(GenericResponse));
        Assert.IsTrue(result.ExecutionSuccessful);
        Assert.AreEqual("User updated successfully", result.Message);
    }
}