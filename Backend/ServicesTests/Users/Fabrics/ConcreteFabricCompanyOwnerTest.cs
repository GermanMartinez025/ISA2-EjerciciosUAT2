using ModelInterface.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Services.Users.FabricUsers;

namespace ServicesTests.Users.Fabrics;

[TestClass]
public class ConcreteFabricCompanyOwnerTest
{

    [TestMethod]
    public void CreateUserTest()
    {
        var concreteFabricCompanyOwner = new ConcreteFabirCompanyOwner();
        var requestAddUser = new RequestAddUser
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "password1A23!"
        };

        var userType = concreteFabricCompanyOwner.CreateConcreteUser(requestAddUser);
        var user = userType.User;

        Assert.IsInstanceOfType(userType, typeof(CompanyOwner));
        Assert.AreEqual(requestAddUser.FirstName, user.FirstName);
        Assert.AreEqual(requestAddUser.LastName, user.LastName);
        Assert.AreEqual(requestAddUser.Email, user.Email);
        Assert.AreEqual(requestAddUser.Password, user.Password);
        Assert.AreEqual(RolesEnum.CompanyOwner, user.Roles[0].Role);
    }
}