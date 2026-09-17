using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelsAPI.Users;

public class ResponseGetUser
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? ProfilePhoto { get; set; }
    public List<string> Roles { get; set; } = new ();
    public string CreationDate { get; set; }
    public ResponseGetUser()
    {
    }
    
    public ResponseGetUser(AUser  user)
    {
        Id = user.Id;
        FullName = user.ToString();
        Email = user.Email;
        ProfilePhoto = user.ProfilePhoto;
        Roles = user.GetRoles();
        //Creation date parse to yyyy-MM-dd HH:mm:ss
        CreationDate = user.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
    }

   
}