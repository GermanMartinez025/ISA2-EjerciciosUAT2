using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using Models.Users.UserTypes;
using Moq;
using Services.Users;

namespace ServicesTests.Users;

[TestClass]
public class AdminServiceTest
{
    private Mock<IAdminRepository> _adminRepository;
    private AdminService _adminService;
    private Admin _admin;

    [TestInitialize]
    public void TestInitialize()
    {
        _adminRepository = new Mock<IAdminRepository>();
        _adminService = new AdminService(_adminRepository.Object);
        _admin = new Admin("Admin", "Admin", "admin@admin.com", "password1@") {UserId = 1};
    }

    [TestMethod]
    public void DeleteAdmin_WhenCalled_ShouldReturnAdmin()
    {
        var secondAdmin = new Admin("AdminTwo", "AdminTwo", "admin2@example.com", "password2@") {UserId = 2};
        _adminRepository.Setup(x => x.GetAll()).Returns([_admin, secondAdmin]);
        var result = _adminService.DeleteAdmin(2);

        Assert.AreEqual(secondAdmin, result);
    }

    [TestMethod]
    public void DeleteAdmin_WhenCalledAndOnlyExistOneAdmin_ShouldThrowException()
    {
        _adminRepository.Setup(x => x.GetAll()).Returns([_admin]);

        Assert.ThrowsException<ForbiddenException>(() => _adminService.DeleteAdmin(1));
    }
    
    [TestMethod]
    public void DeleteAdmin_WhenCalledAndAdminDoesNotExist_ShouldReturnNull()
    {
        var secondAdmin = new Admin("AdminTwo", "AdminTwo", "admin2@example.com", "password2@") {UserId = 2};
        _adminRepository.Setup(x => x.GetAll()).Returns([_admin, secondAdmin]);

        var result = _adminService.DeleteAdmin(3);

        Assert.IsNull(result);
    }
}