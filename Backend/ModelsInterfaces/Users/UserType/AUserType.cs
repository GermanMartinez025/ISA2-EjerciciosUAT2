namespace ModelInterface.Users.UserType;

public abstract class AUserType
{
    public int Id { get; set; }
    public virtual int UserId { get; set; }
    public virtual AUser User { get; set; }
    public abstract RolesEnum Role { get; }
    
    public string FirstName => User.FirstName;
    public string LastName => User.LastName;
    public string Email => User.Email;

    protected AUserType()
    {
    }

    protected AUserType(AUser user)
    {
        User = user;
        UserId = user.Id;
        User.AddRole(this);
    }
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}