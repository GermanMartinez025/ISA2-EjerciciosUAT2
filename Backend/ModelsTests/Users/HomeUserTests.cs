using ModelException;
using ModelInterface.Homes;
using ModelInterface.Users;
using Models.Homes;
using Models.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Moq;

namespace ModelsTests.Users;

[TestClass]
public class HomeUserTests
{
    [TestMethod]
    public void NoParametrizedHomeUserConstructor_WhenCalled_CreatesNewHomeUser()
    {
        var authenticatedUser = new HomeUser(new User());

        Assert.AreEqual(authenticatedUser.User.Roles[0].Role, RolesEnum.HomeUser);
        Assert.IsTrue(authenticatedUser.User.CreationDate < DateTime.Now);
    }


    [TestMethod]
    public void ParametrizedAuthUserConstructorForHomeUser_WhenCalled_CreatesNewHomeUser()
    {
        var Email = "test@test.com";
        var Password = "password123@";
        var FirstName = "First";
        var LastName = "Last";
        var Role = RolesEnum.HomeUser;
        var ProfilePhoto = "test.jpg";

        var homeUser = new HomeUser(FirstName, LastName, Email, Password, ProfilePhoto);
        var authenticatedUser = homeUser.User;

        Assert.AreEqual(authenticatedUser.FirstName, FirstName);
        Assert.AreEqual(authenticatedUser.LastName, LastName);
        Assert.AreEqual(authenticatedUser.Email, Email);
        Assert.AreEqual(authenticatedUser.Password, Password);
        Assert.AreEqual(authenticatedUser.Roles[0].Role, Role);
        Assert.AreEqual(homeUser.ProfilePhoto, ProfilePhoto);
        Assert.AreEqual(homeUser.Notifications.Count, 0);
        Assert.AreEqual(homeUser.Homes.Count, 0);
        Assert.IsTrue(authenticatedUser.CreationDate < DateTime.Now);
    }

    [TestMethod]
    public void InvalidProfilePhoto_ThrowsException()
    {
        var Email = "test@test.com";
        var Password = "password";
        var FirstName = "First";
        var LastName = "Last";
        var Role = RolesEnum.HomeUser;
        var ProfilePhoto = "";
        
        Assert.ThrowsException<BadRequestException>(() => new HomeUser(FirstName, LastName, Email, Password, ProfilePhoto));
    }

    [TestMethod]
    public void SetHomes_WhenCalled_SetsHomes()
    {
        var Email = "test@test.com";
        var Password = "password123@";
        var FirstName = "First";
        var LastName = "Last";
        var ProfilePhoto = "test.jpg";

        var authenticatedUser = new HomeUser(FirstName, LastName, Email, Password, ProfilePhoto);
        var home = new Mock<AHomeMember>();
        
        authenticatedUser.Homes = new List<AHomeMember> {home.Object};
    }
    
    //test Create a homeUser via RequestAddUser
    [TestMethod]
    public void ParametrizedAuthUserConstructorForHomeUser_WhenCalledWithRequestAddUser_CreatesNewHomeUser()
    {
        var Email = "test@test.com";
        var Password = "password123@";
        var FirstName = "First";
        var LastName = "Last";
        var ProfilePhoto = "test.jpg";

        var requestAddUser = new RequestAddUser
        {
            Email = Email,
            Password = Password,
            FirstName = FirstName,
            LastName = LastName,
            ProfilePhoto = ProfilePhoto
        };

        var user = new HomeUser(requestAddUser);
        var authenticatedUser = user.User;

        Assert.AreEqual(authenticatedUser.FirstName, FirstName);
        Assert.AreEqual(authenticatedUser.LastName, LastName);
        Assert.AreEqual(authenticatedUser.Email, Email);
        Assert.AreEqual(authenticatedUser.Password, Password);
        Assert.AreEqual(authenticatedUser.Roles[0].Role, RolesEnum.HomeUser);
        Assert.AreEqual(user.ProfilePhoto, ProfilePhoto);

    }

}