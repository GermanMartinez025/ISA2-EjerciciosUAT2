using ModelInterface.Sessions;
using ModelInterface.Users;
using Models.Users;

namespace Models.Sessions;

public class Session : ISession
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public int UserId { get; set; }
    public AUser User { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session()
    {
    }

    public Session(AUser user)
    {
        Token = Guid.NewGuid();
        UserId = user.Id;
        User = (User)user;
        CreatedAt = DateTime.Now;
    }
}