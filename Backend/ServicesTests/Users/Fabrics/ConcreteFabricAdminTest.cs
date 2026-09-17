using ModelInterface.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Services.Users.FabricUsers;

namespace ServicesTests.Users.Fabrics;

[TestClass]
public class ConcreteFabricAdminTest
{
    [TestMethod]
    public void CreateUserTest()
    {
        var concreteFabricAdmin = new ConcreteFabricAdmin();
        var requestAddUser = new RequestAddUser
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "password1A23!"
        };
        
        var userType = concreteFabricAdmin.CreateConcreteUser(requestAddUser);
        var user = userType.User;
        
        Assert.IsInstanceOfType(userType, typeof(Admin));
        Assert.AreEqual(requestAddUser.FirstName, user.FirstName);
        Assert.AreEqual(requestAddUser.LastName, user.LastName);
        Assert.AreEqual(requestAddUser.Email, user.Email);
        Assert.AreEqual(requestAddUser.Password, user.Password);
        Assert.AreEqual(RolesEnum.Admin, user.Roles[0].Role);
    }
}