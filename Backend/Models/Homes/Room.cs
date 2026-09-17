using ModelInterface.Devices;
using ModelInterface.Homes;

namespace Models.Homes;

public class Room : ARoom
{
    public Room()
    {
        
    }
    public Room(AHome home, string name) : base(home, name)
    {
    }
}