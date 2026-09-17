using ModelException;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Services.Users.FabricUsers;

namespace ServicesTests.Users.Fabrics;

[TestClass]
public class FabricUserTest
{
    private RequestAddUser _requestAddUser;

    [TestInitialize]
    public void Initialize()
    {
        _requestAddUser = new RequestAddUser
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "password1A23!",
            Role = "Admin"
        };
    }

    [TestMethod]
    public void CreateUserWithRoleAdmin_ShouldReturnInstanceOfAdmin()
    {
        var user = FabricUser.GetFabricUser(_requestAddUser.Role).CreateConcreteUser(_requestAddUser);

        Assert.IsInstanceOfType(user, typeof(Admin));
    }

    [TestMethod]
    public void CreateUserWithRoleCompanyOwner_ShouldReturnInstanceOfCompanyOwner()
    {
        _requestAddUser.Role = "CompanyOwner";
        
        var user = FabricUser.GetFabricUser(_requestAddUser.Role).CreateConcreteUser(_requestAddUser);

        Assert.IsInstanceOfType(user, typeof(CompanyOwner));
    }

    [TestMethod]
    public void CreateUserWithRoleHomeUser_ShouldReturnInstanceOfHomeUser()
    {
        _requestAddUser.Role = "HomeUser";
        _requestAddUser.ProfilePhoto = "profilePhoto.jpg";

        var user = FabricUser.GetFabricUser(_requestAddUser.Role).CreateConcreteUser(_requestAddUser);

        Assert.IsInstanceOfType(user, typeof(HomeUser));
    }

    [TestMethod]
    public void CreateUserWithRoleUnknown_ShouldThrowException()
    {
        _requestAddUser.Role = "Pepito";

        Assert.ThrowsException<BadRequestException>(
            () => FabricUser.GetFabricUser(_requestAddUser.Role).CreateConcreteUser(_requestAddUser));
    }

}