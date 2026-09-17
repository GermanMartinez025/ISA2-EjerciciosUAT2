using ModelInterface.Devices;

namespace ModelsAPI.Devices;

public class ResponseDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; }
    public string CompanyName { get; set; }
    public string DeviceType { get; set; }
    public string MainPhoto { get; set; }
    
    public ResponseDevice()
    {
        
    }
    
    public ResponseDevice(IDevice device)
    {
        Id = device.Id;
        Name = device.Name;
        Model = device.ModelNumber;
        Description = device.Description;
        Photos = device.PhotosUrls;
        CompanyName = device.Company.Name;
        DeviceType = device.DeviceType.ToString();
        MainPhoto = device.MainPhoto;
        
    }
}