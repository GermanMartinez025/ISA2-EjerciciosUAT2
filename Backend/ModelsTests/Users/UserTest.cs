using ModelException;
using ModelInterface.Users;
using Models.Users;
using Models.Users.UserTypes;

namespace ModelsTests.Users;

[TestClass]
public class UserTest
{
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithInvalidName_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "password";
        var FirstName = "First1";
        var LastName = "Last";

        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithEmptyName_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "password";
        var FirstName = "";
        var LastName = "Last";
    
        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithInvalidLastName_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "password";
        var FirstName = "First";
        var LastName = "Last1";
        
        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithEmptyLastName_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "password";
        var FirstName = "First";
        var LastName = "";

        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithInvalidEmail_ThrowsArgumentException()
    {
        var Email = "testtest.com";
        var Password = "password";
        var FirstName = "First";
        var LastName = "Last";
        
        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithEmptyEmail_ThrowsArgumentException()
    {
        var Email = "";
        var Password = "password";
        var FirstName = "First";
        var LastName = "Last";

        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithEmptyPassword_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "";
        var FirstName = "First";
        var LastName = "Last";

        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }

    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWith5CharPassword_ThrowsArgumentException()
    {
        var Email = "test@test.com";
        var Password = "passw";
        var FirstName = "First";
        var LastName = "Last";

        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }

    [TestMethod]
    public void ParametrizedAuthUserConstructor_WhenCalledWithPasswordWithoutSymbol_ThrowsArgumentException()
    {
        var Email = "test@example.com";
        var Password = "password";
        var FirstName = "First";
        var LastName = "Last";
        
        Assert.ThrowsException<BadRequestException>(() => new User(FirstName, LastName, Email, Password));
    }
    
    [TestMethod]
    public void HasRole_WhenCalledWithRole_ReturnsTrue()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        var role = RolesEnum.CompanyOwner;
        user.AddRole(role);

        Assert.IsTrue(user.HasRole(role.ToString()));

    }
    
    [TestMethod]
    public void GetUser_WhenCalledWithUserRole_ReturnsUser()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        user.AddRole(RolesEnum.Admin);
        var userRole = user.Roles;

        Assert.AreEqual(user.Id, userRole[0].UserId);
        Assert.AreEqual(user, userRole[0].User);
    }
    
    [TestMethod]
    public void AddRole_WhenCalledWithRole_UpdateRole()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        var roleToAdd = "HomeUser";
        
        user.Update("Role",roleToAdd);
        //TODO: Fix this test
        Assert.IsTrue( user.Roles.Any(r => r.Role.ToString() == roleToAdd));
        Assert.AreEqual(1, user.Roles.Count);
    }
    
    [TestMethod]
    public void AddRole_WhenCalledWithInvalidRole_ThrowsArgumentException()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        var roleToAdd = "Pium";

        Assert.ThrowsException<ConflictException>(() => user.Update("Role", roleToAdd));
    }
    
    [TestMethod]
    public void HasRole_WhenCalledWithInvalidRole_ReturnsFalse()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        var role = "Pium";

        Assert.IsFalse(user.HasRole(role));
    }
    
    //IsAdmin
    [TestMethod]
    public void IsAdmin_WhenCalled_ReturnsTrue()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        user.AddRole(RolesEnum.Admin);

        Assert.IsTrue(user.IsAdmin());
    }
    
    [TestMethod]
    public void IsCompanyOwner_WhenCalled_ReturnsTrue()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        user.AddRole(RolesEnum.CompanyOwner);

        Assert.IsTrue(user.IsCompanyOwner());
    }
    
    [TestMethod]
    public void AddRole_WhenCalledWithCompanyOwnerRole_ThrowsArgumentException()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        user.AddRole(RolesEnum.CompanyOwner);
        
        var roleToAdd = "CompanyOwner";

        Assert.ThrowsException<ConflictException>(() => user.Update("Role", roleToAdd));
    }
    
    [TestMethod]
    public void AddRole_WhenCalledWithAdminRole_ThrowsArgumentException()
    {
        var Email = "test@example.com";
        var Password = "password@123";
        var FirstName = "First";
        var LastName = "Last";
        var user = new User(FirstName, LastName, Email, Password);
        user.AddRole(RolesEnum.Admin);

        var roleToAdd = "Admin";

        Assert.ThrowsException<ConflictException>(() => user.Update("Role", roleToAdd));
    }





}