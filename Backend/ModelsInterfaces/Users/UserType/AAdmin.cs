namespace ModelInterface.Users.UserType;

public abstract class AAdmin : AUserType
{
    public override RolesEnum Role { get; } = RolesEnum.Admin;

    protected AAdmin()
    {
    }

    protected AAdmin(AUser user) : base(user)
    {
    }

}