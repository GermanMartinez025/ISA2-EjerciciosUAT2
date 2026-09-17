using ModelInterface.Users;

namespace ModelInterface.Sessions;

public interface ISession
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public int UserId { get; set; }
    public AUser User { get; set; }
    public DateTime CreatedAt { get; set; }
}