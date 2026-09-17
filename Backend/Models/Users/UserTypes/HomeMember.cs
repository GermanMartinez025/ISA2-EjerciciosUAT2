using ModelInterface.Homes;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace Models.Users.UserTypes;

public class HomeMember : AHomeMember
{

    public HomeMember()
    {
    }
    
    public HomeMember (AHome home, AHomeUser aHomeUser) : base(home, aHomeUser)
    {
        
    }
    
}