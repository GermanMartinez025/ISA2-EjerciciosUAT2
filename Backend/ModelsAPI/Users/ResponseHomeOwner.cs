using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelsAPI.Users;

public class ResponseHomeOwner
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public ResponseHomeOwner(AHomeUser user)
    {
        UserId = user.Id;
        Name = user.User.FirstName + " " + user.User.LastName;
        Email = user.User.Email;
    }
}