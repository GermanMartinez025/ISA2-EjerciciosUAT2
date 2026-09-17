namespace ModelsAPI.Homes;

public class RequestCreateHome
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public Geolocation Geolocation { get; set; }
    public int MaxMembers { get; set; }
    
    public RequestCreateHome()
    {
        Name = "";
        Address = new Address();
        Geolocation = new Geolocation();
        MaxMembers = 0;
    }
    
    public RequestCreateHome(string name, Address address, Geolocation geolocation, int maxMembers)
    {
        Name = name;
        Address = address;
        Geolocation = geolocation;
        MaxMembers = maxMembers;
    }
}

public class Address
{
    public string MainStreet { get; set; }
    public int DoorNumber { get; set; }
}

public class Geolocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
