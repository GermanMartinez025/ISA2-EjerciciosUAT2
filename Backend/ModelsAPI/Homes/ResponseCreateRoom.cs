using ModelInterface.Homes;

namespace ModelsAPI.Homes;

public class ResponseCreateRoom
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int HomeId { get; set; }

    public ResponseCreateRoom(ARoom room)
    {
        Id = room.Id;
        Name = room.Name;
        HomeId = room.HomeId;

    }
    
}