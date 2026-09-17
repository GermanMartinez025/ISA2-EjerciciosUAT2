using ModelInterface.Users;
using Models.Users.UserTypes;
using ModelsAPI.Users;
using Services.Users.FabricUsers;

namespace ServicesTests.Users.Fabrics;

[TestClass]
public class ConcreteFabricHomeUserTest
{
    [TestMethod]
    public void CreateUserTest()
    {
        var concreteFabricHomeUser = new ConcreteFabricHomeUser();
        var requestAddUser = new RequestAddUser
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "password1A23!",
            ProfilePhoto = "profilePhoto.jpg"
        };

        var userType = concreteFabricHomeUser.CreateConcreteUser(requestAddUser);
        var user = userType.User;

        Assert.IsInstanceOfType(userType, typeof(HomeUser));
        Assert.AreEqual(requestAddUser.FirstName, user.FirstName);
        Assert.AreEqual(requestAddUser.LastName, user.LastName);
        Assert.AreEqual(requestAddUser.Email, user.Email);
        Assert.AreEqual(requestAddUser.Password, user.Password);
        Assert.AreEqual(requestAddUser.ProfilePhoto, user.HomeUser.ProfilePhoto);
        Assert.AreEqual(RolesEnum.HomeUser, user.Roles[0].Role);
    }
}