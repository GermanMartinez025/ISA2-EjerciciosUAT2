using ModelInterface.Homes;
using ModelsAPI.Users;
using ModelInterface.Users;

namespace ModelsAPI.Homes;

public class ResponseCreateHome
{
    public int HomeId { get; set; }
    public string Name { get; set; }
    public string MainStreet { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
    public ResponseHomeOwner Owner { get; set; }
    
    public ResponseCreateHome(){}

    public ResponseCreateHome(AHome home)
    {
        HomeId = home.Id;
        Name = home.Name;
        MainStreet = home.MainStreet;
        DoorNumber = home.DoorNumber;
        Latitude = home.Latitude;
        Longitude = home.Longitude;
        MaxMembers = home.MaxMembers;
        Owner = new ResponseHomeOwner(home.Owner);
        
    }
    
    
}

