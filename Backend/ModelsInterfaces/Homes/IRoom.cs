using ModelInterface.Devices;

namespace ModelInterface.Homes;

public interface IRoom
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int HomeId { get; set; }
    public AHome Home { get; set; }
    public List<AHomeDevice> Devices { get; set; }
    
}