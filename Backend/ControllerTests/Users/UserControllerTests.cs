using Controllers.Users;
using ModelException;
using ModelInterface.Users;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Moq;
using ServicesInterfaces;
using ServicesInterfaces.Sessions;
using ServicesInterfaces.Users;

namespace ControllerTests.Users;

[TestClass]
public class UserControllerTests
{
    private Mock<IUserService> _userService;
    private UserController _userController;
    private Mock<ISessionService> _sessionService;
    private RequestAddUser _requestAddUser;
    private List<string> _adminRoles = new List<string>() { "Admin" };
    private List<string> _homeUserRoles = new List<string>() { "HomeUser" };
    private List<string> _companyOwnerRoles = new List<string>() { "CompanyOwner" };
    private List<string> _noSessionRoles = new List<string>() { "NoSession" };

    [TestInitialize]
    public void Initialize()
    {
        _userService = new Mock<IUserService>();
        _sessionService = new Mock<ISessionService>();
        _userService.Setup(m => m.AddUser(It.IsAny<RequestAddUser>(), It.IsAny<List<string>>()));
        _userService.Setup(m => m.DeleteUser(It.IsAny<int>()));
        _userController = new UserController(_userService.Object, _sessionService.Object);
        
        _requestAddUser = new RequestAddUser()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "password123@",
            Role = "HomeUser",
            ProfilePhoto = "photo"
        };
    }

    [TestMethod]
    public void AddAdmin_ShouldAddAdmin()
    {
        _requestAddUser.Role = "Admin";
        _sessionService.Setup(m => m.GetRolesFromSession(It.IsAny<string>())).Returns(_adminRoles);
        _userService.Setup(m => m.AddUser(It.IsAny<RequestAddUser>(), It.IsAny<List<string>>())).Returns(new User());
        _userController.AddUser(_requestAddUser, "token");

        _userService.Verify(
            m => m.AddUser(It.Is<RequestAddUser>(x =>
                x.FirstName == "John" && x.LastName == "Doe" && x.Email == "john@example.com" &&
                x.Password == "password123@"), It.IsAny<List<string>>()), Times.Once());
    }

    [TestMethod]
    public void DeleteAdmin_ShouldDeleteAdmin()
    {
        _sessionService.Setup(m => m.GetUserFromSession(It.IsAny<string>())).Returns(new User("Admin", "Admin", "admin@example.com", "password1@"));
        _userController.DeleteUser(1, "token");

        _userService.Verify(m => m.DeleteUser(It.IsAny<int>()), Times.Once());
    }

    [TestMethod]
    public void GetUsers_ShouldReturnUsers()
    {
        var user1 = new User("Admin", "Admin", "admin@example.com", "password1@");
        var user2 = new User("HomeUser", "HomeUser", "user@example.com", "password1@");
        user1.AddRole(RolesEnum.Admin);
        user2.AddRole(RolesEnum.HomeUser);
        
        var users = new List<AUser>()
        {
            user1,
            user2
        };

        _userService.Setup(m => m.GetUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(users);
        _userService.Setup(m => m.GetAmountOfUsers(It.IsAny<Dictionary<string, object>>())).Returns(2);

        var response = _userController.GetUsers(1, 10, null, null);

        Assert.AreEqual(2, response.TotalUsers);
        Assert.AreEqual(1, response.actualPage);
        Assert.AreEqual(1, response.totalPages);
        Assert.AreEqual(2, response.Users.Count);

        Assert.AreEqual("Admin Admin", response.Users[0].FullName);
        Assert.AreEqual("Admin", response.Users[0].Roles[0]);
        Assert.IsNotNull(response.Users[0].CreationDate);
    }

    [TestMethod]
    public void GetUsersWithSpecificRoles_ShouldReturnUsers()
    {
        var users = new List<AUser>()
        {
            new User("Admin", "Admin", "admin@example.com", "password1@"),
        };


        _userService.Setup(m => m.GetUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(users);
        _userService.Setup(m => m.GetAmountOfUsers(It.IsAny<Dictionary<string, object>>())).Returns(1);

        var response = _userController.GetUsers(1, 10, _adminRoles, null);

        Assert.AreEqual(1, response.TotalUsers);
        Assert.AreEqual(1, response.actualPage);
        Assert.AreEqual(1, response.totalPages);
        Assert.AreEqual(1, response.Users.Count);
    }

    [TestMethod]
    public void GetUsersWithSpecificNames_ShouldReturnUsers()
    {
        var users = new List<AUser>()
        {
            new User("Admin", "Admin", "admin@admin.com", "password1@"),
        };

        _userService.Setup(m => m.GetUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(users);
        _userService.Setup(m => m.GetAmountOfUsers(It.IsAny<Dictionary<string, object>>())).Returns(1);

        var response = _userController.GetUsers(1, 10, null, new List<string>() { "John Doe" });

        Assert.AreEqual(1, response.TotalUsers);
    }

   
    
    [TestMethod]
    public void DeleteYourself_ShouldThrowException()
    {
        var admin = new User("Admin", "Admin", "admin@example.com", "password1@") {Id = 1};
        _sessionService.Setup(m => m.GetUserFromSession(It.IsAny<string>())).Returns(admin);

        Assert.ThrowsException<ForbiddenException>(() => _userController.DeleteUser(1, "token"));
    }
    
    [TestMethod]
    public void AddRole_ShouldAddRole()
    {
        var user = new User("Admin", "Admin", "admin@example.com", "password1@");
        _sessionService.Setup(m => m.GetUserFromSession(It.IsAny<string>())).Returns(user);
        _sessionService.Setup(m => m.ValidSession(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _userService.Setup(m => m.UpdateUser(It.IsAny<int>(), It.IsAny<RequestUpdateUser>())).Returns(user);

        var requestAddRole = new RequestUpdateUser() { Role = "HomeUser" };
        
        _userController.UpdateUser(requestAddRole, "token");
        
        _userService.Verify(m => m.UpdateUser(It.IsAny<int>(), It.IsAny<RequestUpdateUser>()), Times.Once());
    }


}