using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Moq;
using Services.Users;
using ServicesInterfaces.Users;

namespace ServicesTests.Users;

[TestClass]
public class UserServiceTest
{
    private Mock<IUserRepository> _userRepository;
    private UserService _userService;
    private RequestAddUser _requestAddUser = new (){FirstName = "User", LastName = "User", Email="user@example.com", Password= "password1@"};
    private List<string> _sessionRoles = new ();
    
    [TestInitialize]
    public void TestInitialize()
    {
        _userRepository = new Mock<IUserRepository>();
        _userService = new UserService(_userRepository.Object);
        
    }

    [TestMethod]
    public void AddUserWithRoleAdmin_shouldCallAdminService()
    {
        var mockAdmin = new Mock<AAdmin>();

        var role = "Admin";
        _requestAddUser.Role = role;
        _sessionRoles.Add(role);
        
        _userService.AddUser(_requestAddUser, _sessionRoles);
        
        _userRepository.Verify(x => x.Create(It.IsAny<AUser>()), Times.Once);
    }

    [TestMethod]
    public void AddUserWithRoleHomeUser_shouldCallHomeUserService()
    {
        var mockHomeUser = new Mock<AHomeUser>();
        
        var role = "HomeUser";
        _requestAddUser.Role = role;
        _requestAddUser.ProfilePhoto = "photo.jpg";
        _sessionRoles.Add("NoSession");

        _userService.AddUser(_requestAddUser, _sessionRoles);
        
        _userRepository.Verify(x => x.Create(It.IsAny<AUser>()), Times.Once);
    }

    [TestMethod]
    public void AddUserWithRoleCompanyOwner_shouldCallCompanyOwnerService()
    {
        var mockCompanyOwner = new Mock<CompanyOwner>();
        
        var role = "CompanyOwner";
        _requestAddUser.Role = role;
        _sessionRoles.Add("Admin");
        
        _userService.AddUser(_requestAddUser, _sessionRoles);
        
        _userRepository.Verify(x => x.Create(It.IsAny<AUser>()), Times.Once);
    }

    [TestMethod]
    public void DeleteUserWithRoleAdmin_shouldCallAdminService()
    {
        var mockAdmin = new Mock<AAdmin>();
        mockAdmin.Setup(x => x.User.IsAdmin()).Returns(true);
        _userRepository.Setup(x => x.GetById(mockAdmin.Object.UserId)).Returns(mockAdmin.Object.User);

        _userService.DeleteUser(mockAdmin.Object.UserId);

        _userRepository.Verify(x => x.Delete(mockAdmin.Object.User), Times.Once);
    }
    
    [TestMethod]
    public void GetUsers_shouldCallUserRepository()
    {
        var page = 1;
        var pageSize = 1;
        _userRepository.Setup(x => x.GetPaginated(page, pageSize, null)).Returns(new List<AUser>());
        
        _userService.GetUsers(page, pageSize, null);
        
        _userRepository.Verify(x => x.GetPaginated(page, pageSize, null), Times.Once);
    }
    
    [TestMethod]
    public void GetAmountOfUsers_shouldCallUserRepository()
    {
        _userRepository.Setup(x => x.Count()).Returns(1);
        
        _userService.GetAmountOfUsers(null);
        
        _userRepository.Verify(x => x.Count(), Times.Once);
    }
    
    [TestMethod]
    public void GetUsersWithSpecificRoles_shouldCallUserRepository()
    {
        var page = 1;
        var pageSize = 1;
        var roles = new List<string>() { "Admin" };
        var filter = new Dictionary<string, object>() { { "role", roles } };
        _userRepository.Setup(x => x.GetPaginated(page, pageSize, filter)).Returns(new List<AUser>());
        
        _userService.GetUsers(page, pageSize, filter);
        
        _userRepository.Verify(x => x.GetPaginated(page, pageSize, filter), Times.Once);
    }
    
    [TestMethod]
    public void GetAmountOfUsersWithFilter_shouldCallUserRepository()
    {
        var roles = new List<string>() { "Admin" };
        var filter = new Dictionary<string, object>() { { "role", roles } };
        _userRepository.Setup(x => x.Count(filter)).Returns(1);
        
        _userService.GetAmountOfUsers(filter);
        
        _userRepository.Verify(x => x.Count(filter), Times.Once);
    }

    
    [TestMethod]
    public void ValidateCredentials_shouldCallUserRepository()
    {
        var email = "admin@example.com";
        var password = "password1@";
        var expetedUser = new User
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@example.com",
            Password = "password1@"

        };
        _userRepository
            .Setup(x => x.ValidateCredentials(email, password))
            .Returns(expetedUser);

        _userService.ValidateCredentials(email, password);
        
        _userRepository.Verify(x => x.ValidateCredentials(email, password), Times.Once);
        
    }

    [TestMethod]
    public void GetByEmail_shouldCallUserRepository()
    {
        var email = "admin@example.com";
        var mockAdmin = new Admin("Admin", "Admin", email, "password1@");
        _userRepository.Setup(x => x.GetByEmail(email)).Returns(mockAdmin.User);

        _userService.GetByEmail(email);

        _userRepository.Verify(x => x.GetByEmail(email), Times.Once);
    }

    [TestMethod]
    public void GetById_shouldCallUserRepository()
    {
        var userId = 1;
        var mockAdmin = new Admin("Admin", "Admin", "admin@example.com", "password1@");

        _userRepository.Setup(x => x.GetById(userId)).Returns(mockAdmin.User);

        _userService.GetById(userId);
    }

    [TestMethod]
    public void CheckAvailableEmail_shouldThrowExceptionWhenEmailIsAlreadyInUse()
    {
        var email = "user@example.com";
        var mockAdmin = new Admin("Admin", "Admin", email, "password1@");
        _sessionRoles.Add("Admin");
        _requestAddUser.Role = "Admin";
        

        _userRepository.Setup(x => x.GetByEmail(email)).Returns(mockAdmin.User);

        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }
    
     [TestMethod]
    public void CreateAdminWithHomeUserRole_ShouldThrowException()
    {
        _requestAddUser.Role = "Admin";
        _sessionRoles.Add("HomeUser");
        
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }

    [TestMethod]
    public void CreateHomeUserWithAdminRole_ShouldThrowException()
    {
        _requestAddUser.Role = "HomeUser";
        _sessionRoles.Add("Admin");
        
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }

    [TestMethod]
    public void CreateHomeUserWithCompanyOwnerRole_ShouldThrowException()
    {
        _requestAddUser.Role = "HomeUser";
        _sessionRoles.Add("CompanyOwner");
        
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }

    [TestMethod]
    public void CreateAdminWithCompanyOwnerRole_ShouldThrowException()
    {
        _requestAddUser.Role = "Admin";
        _sessionRoles.Add("CompanyOwner");
        
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }

    [TestMethod]
    public void CreateHomeUserWithNoSessionRole_ShouldNotThrowException()

    {
        _requestAddUser.Role = "HomeUser";
        _requestAddUser.ProfilePhoto = "profile.png";
        _sessionRoles.Add("NoSession");

        _userService.AddUser(_requestAddUser, _sessionRoles);
    }

    [TestMethod]
    public void CreateAdminWithNoSessionRole_ShouldThrowException()
    {
        _requestAddUser.Role = "Admin";
        _sessionRoles.Add("NoSession");
        
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }
    
    [TestMethod]
    public void UpdateUser_NoUpdated_ShouldThrowException()
    {
        var userId = 1;
        var request = new RequestUpdateUser();
        var mockAdmin = new Mock<Admin>();
        _userRepository.Setup(x => x.GetById(userId)).Returns(mockAdmin.Object.User);


        Assert.ThrowsException<NotFoundException>(() => _userService.UpdateUser(userId, request));
    }
    
    [TestMethod]
    public void UpdateUser_Updated_ShouldReturnUser()
    {
        var userId = 1;
        var request = new RequestUpdateUser();
        request.Role = "HomeUser";
        var mockAdmin = new Admin("Admin", "Admin", "admin@admin.com", "password1@");
        _userRepository.Setup(x => x.GetById(userId)).Returns(mockAdmin.User);
        _userRepository.Setup(x => x.Update(mockAdmin.User)).Returns(mockAdmin.User);

        var result = _userService.UpdateUser(userId, request);

        _userRepository.Verify(x => x.Update(mockAdmin.User), Times.Once);
    }

    [TestMethod]
    public void ValidateRoles_ShouldThrowExceptionWhenRoleIsNotAllowed()
    {
        _sessionRoles.Add("Admin");
        _requestAddUser.Role = "HomeUser";
    
        Assert.ThrowsException<ConflictException>(() => _userService.AddUser(_requestAddUser, _sessionRoles));
    }
    
    [TestMethod]
    public void DeleteUser_ShouldThrowExceptionWhenUserIsNotAdmin()
    {
        var mockUser = new Mock<AUser>();
        mockUser.Setup(x => x.IsAdmin()).Returns(false);
        _userRepository.Setup(x => x.GetById(1)).Returns(mockUser.Object);

        Assert.ThrowsException<ConflictException>(() => _userService.DeleteUser(1));
    }

    [TestMethod]
    public void GetById_ShouldThrowExceptionWhenUserNotFound()
    {
        var userId = 1;
        _userRepository.Setup(x => x.GetById(userId)).Returns((AUser)null);

        Assert.ThrowsException<NotFoundException>(() => _userService.GetById(userId));
    }

    [TestMethod]
    public void GetByEmail_ShouldThrowExceptionWhenUserNotFound()
    {
        var email = "nonexistent@example.com";
        _userRepository.Setup(x => x.GetByEmail(email)).Returns((AUser)null);

        Assert.ThrowsException<NotFoundException>(() => _userService.GetByEmail(email));
    }
    

    [TestMethod]
    public void ValidateCredentials_ShouldNotThrowException_WhenCredentialsAreValid()
    {
        string email = "test@example.com";
        string password = "validPassword";

        var mockUser = new Mock<AUser>();
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.ValidateCredentials(email, password)).Returns(mockUser.Object);

        var userService = new UserService(mockUserRepository.Object);
        
        try
        {
            userService.ValidateCredentials(email, password);
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.Fail("ValidateCredentials threw an ArgumentOutOfRangeException unexpectedly");
        }
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void ValidateCredentials_ShouldThrowArgumentOutOfRangeException_WhenCredentialsAreInvalid()
    {
        string email = "invalid@example.com";
        string password = "wrongPassword";

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(repo => repo.ValidateCredentials(email, password)).Returns((AUser)null); 
        
        var userService = new UserService(mockUserRepository.Object);

       
        userService.ValidateCredentials(email, password); 
    }

    [TestMethod]
    public void UpdateUser_ShouldThrowException_WhenNoFieldsToUpdate()
    {
        var userId = 1;
        var request = new RequestUpdateUser(); 

        var mockUser = new Mock<AUser>();
        mockUser.Setup(x => x.Id).Returns(userId);

        _userRepository.Setup(x => x.GetById(userId)).Returns(mockUser.Object);

        Assert.ThrowsException<ConflictException>(() => _userService.UpdateUser(userId, request));
    }




}