using ModelInterface.Users;

namespace ModelsAPI.Users;

public class ResponseGetHomeMember
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ProfilePhoto { get; set; }
    public bool IsNotifiable { get; set; }
    public bool ListDevices { get; set; }
    public bool ListUsers { get; set; }
    
    public ResponseGetHomeMember()
    {
        
    }
    
    public ResponseGetHomeMember(AHomeMember aHomeMember)
    {
        FirstName = aHomeMember.AHomeUser.User.FirstName;
        LastName = aHomeMember.AHomeUser.User.LastName;
        Email = aHomeMember.AHomeUser.User.Email;
        ProfilePhoto = aHomeMember.AHomeUser.ProfilePhoto;
        IsNotifiable = aHomeMember.Notifiable;
        ListDevices = aHomeMember.ListDevices;
        ListUsers = aHomeMember.AddDevices;
    }
    
}