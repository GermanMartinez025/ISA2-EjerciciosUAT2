using ModelException;
using ModelInterface.Devices;

namespace ModelInterface.Homes;

public abstract class ARoom : IRoom

{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual int HomeId { get; set; }
    
    public virtual AHome Home { get; set; }
    public virtual List<AHomeDevice> Devices { get; set; }

    public ARoom()
    {
        
    }
    
    public ARoom(AHome home, string name)
    {
        ValidateHome(home);
        ValidateName(name);
        Home = home;
        HomeId = home.Id;
        Name = name;
        Devices = new List<AHomeDevice>();
    }

    private static void ValidateHome(AHome home)
    {
        if(home == null)
        {
            throw new BadRequestException(nameof(home));
        }
    }
    
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("The name cannot be null or empty.");
        }
    }
    
    
}