using ModelException;
using ModelInterface.Devices;
using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelInterface.Homes;

public abstract class AHome : IHome
{
    public virtual int Id { get; set; }
    public string MainStreet { get; set; }
    public int DoorNumber { get; set; }
    
    public string Name {get; set;}
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public virtual int MaxMembers { get; set; }
    public virtual List<AHomeMember> Members { get; set; } = new List<AHomeMember>();
    public virtual List<AHomeUser> Users { get; set; } = new List<AHomeUser>();
    public virtual List<AHomeDevice> Devices { get; set; } = new List<AHomeDevice>();
    public virtual AHomeUser Owner { get; set; }
    public virtual List<ARoom> Rooms { get; set; } = new List<ARoom>();
    public virtual int HomeOwnerId { get; set; }


    protected AHome() {}
    
    protected AHome(string mainStreet, int doorNumber, string name, double latitude, double longitude, int maxMembers, AHomeUser owner)
    {
        ValidateAll(mainStreet, doorNumber, name, latitude, longitude, maxMembers, owner);
        
        MainStreet = mainStreet;
        DoorNumber = doorNumber;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        MaxMembers = maxMembers;
        Members = new List<AHomeMember>();
        Devices = new List<AHomeDevice>();
        Owner = owner;
        HomeOwnerId = owner.UserId;
    }

    public abstract void NotifyMembers(AHomeDevice device, EventType eventType);
    
    private static void ValidateAll(string mainStreet, int doorNumber, string name, double latitude, double longitude, int maxMembers, AHomeUser owner)
    {
        ValidateMaxMember(maxMembers, owner);
        ValidateMainStreet(mainStreet);
        ValidateDoorNumber(doorNumber);
        ValidateName(name);
        ValidateLatitude(latitude);
        ValidateLongitude(longitude);
    }
    
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("The name cannot be null or empty.");
        }
    }

    private static void ValidateMaxMember(int maxMembers, AHomeUser owner)
    {
        if (maxMembers < 1)
        {
            throw new BadRequestException("The maximum number of members must be greater than 0.");
        }
        if (owner == null)
        {
            throw new NotFoundException("The home owner cannot be null.");
        }
    }
    
    private static void ValidateMainStreet(string mainStreet)
    {
        if (string.IsNullOrWhiteSpace(mainStreet))
        {
            throw new BadRequestException("The main street cannot be null or empty.");
        }
    }
    private static void ValidateDoorNumber(int doorNumber)
    {
        if (doorNumber < 1)
        {
            throw new BadRequestException("The door number must be greater than 0.");
        }
    }
    private static void ValidateLatitude(double latitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new BadRequestException("The latitude must be between -90 and 90.");
        }
    }

    private static void ValidateLongitude(double longitude)
    {
        if (longitude < -180 || longitude > 180)
        {
            throw new BadRequestException("The longitude must be between -180 and 180.");
        }
    }
    
}