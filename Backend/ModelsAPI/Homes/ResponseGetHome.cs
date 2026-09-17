using ModelInterface.Homes;

namespace ModelsAPI.Homes;

public class ResponseGetHome
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MainStreet { get; set; }
    public int DoorNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
    public string Owner { get; set; }
    
    public ResponseGetHome()
    {
        
    }
    public ResponseGetHome(IHome home)
    {
        Id = home.Id;
        Name = home.Name;
        MainStreet = home.MainStreet;
        DoorNumber = home.DoorNumber;
        Latitude = home.Latitude;
        Longitude = home.Longitude;
        MaxMembers = home.MaxMembers;
        Owner = home.Owner.User.FirstName+" "+home.Owner.User.LastName;
    }
}