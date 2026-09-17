using ModelInterface.Notifications;

namespace ModelInterface.Users.UserType;

public abstract class AHomeUser : AUserType
{
    public override RolesEnum Role { get; } = RolesEnum.HomeUser;
    public string? ProfilePhoto { get; set; }
    public virtual List<ANotification> Notifications { get; set; } = new ();
    public List<AHomeMember> Homes { get; set; } = new ();

    protected AHomeUser()
    {
       
    }
    
    protected AHomeUser(AUser user) : base(user)
    {
    }
    
    protected AHomeUser(AUser user, string profilePhoto) : base(user)
    {
        ProfilePhoto = profilePhoto;
    }
}