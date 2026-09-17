using ModelInterface;
using ModelInterface.Users;
using Models.Users;
using Models.Users.UserTypes;

namespace ModelsTests.Users;

[TestClass]
public class CompanyOwnerTests
{
    [TestMethod]
    public void NoParametrizedCompanyOwnerConstructor_WhenCalled_CreatesNewCompanyOwner()
    {
        var authenticatedUser = new CompanyOwner();

        Assert.AreEqual(typeof(CompanyOwner), authenticatedUser.GetType());
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructorForCompanyOwner_WhenCalled_CreatesNewCompanyOwner()
    {
        var Email = "test@test.com";
        var Password = "password123@";
        var FirstName = "First";
        var LastName = "Last";
        var Role = RolesEnum.CompanyOwner;

        var user = new CompanyOwner(FirstName, LastName, Email, Password);
        var authenticatedUser = user.User;

        Assert.AreEqual(authenticatedUser.FirstName, FirstName);
        Assert.AreEqual(authenticatedUser.LastName, LastName);
        Assert.AreEqual(authenticatedUser.Email, Email);
        Assert.AreEqual(authenticatedUser.Password, Password);
        Assert.AreEqual(authenticatedUser.Roles[0].Role, Role);
        Assert.IsTrue(authenticatedUser.CreationDate < DateTime.Now);
    }
}