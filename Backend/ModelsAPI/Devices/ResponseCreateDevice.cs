using ModelInterface.Devices;

namespace ModelsAPI.Devices;

public class ResponseCreateDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string TypeDevice { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; }
    
    public ResponseCreateDevice()
    {
    }
    
    public ResponseCreateDevice(IDevice device)
    {
        Id = device.Id;
        TypeDevice = device.DeviceType.ToString();
        Name = device.Name;
        ModelNumber = device.ModelNumber;
        Description = device.Description;
        Photos = device.PhotosUrls;
    }
    
}