using ModelInterface.Users;
using Models.Users.UserTypes;

namespace ModelsTests.Users;

[TestClass]
public class AdminTest
{
    [TestMethod]
    public void NoParametrizedAdminConstructor_WhenCalled_CreatesNewAdmin()
    {
        var authenticatedUser = new Admin();

        Assert.AreEqual(typeof(Admin), authenticatedUser.GetType());
    }

    [TestMethod]
    public void ParametrizedAuthUserConstructorForAdmin_WhenCalled_CreatesNewAdmin()
    {
        var name = "John";
        var lastName = "Doe";
        var email = "john@example.com";
        var password = "password1@";
        var user = new Admin(name, lastName, email, password);
        var authenticatedUser = user.User;

        Assert.AreEqual(authenticatedUser.FirstName, name);
        Assert.AreEqual(authenticatedUser.LastName, lastName);
        Assert.AreEqual(authenticatedUser.Email, email);
        Assert.AreEqual(authenticatedUser.Password, password);
        Assert.AreEqual(authenticatedUser.Roles[0].Role, RolesEnum.Admin);
    }
}