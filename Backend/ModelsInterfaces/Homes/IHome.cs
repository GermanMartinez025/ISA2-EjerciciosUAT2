using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelInterface.Homes;

public interface IHome
{
    public int Id { get; set; }
    public string MainStreet { get; set; }
    public int DoorNumber { get; set; }
    public string Name {get; set;}
    public double Latitude { get; set; } 
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
    public List<AHomeMember> Members { get; set; }
    public AHomeUser Owner { get; set; }
    public List<ARoom> Rooms { get; set; }
}