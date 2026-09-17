using ModelInterface.Companys;

namespace ModelInterface.Devices;

public interface IDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<DevicePhoto> Photos { get; set; }
    public ACompany Company { get; set; }
    public DeviceEnum DeviceType { get; set; }
    public bool CanRepeatLastEvent { get; }
    public string MainPhoto { get; set; }
    public List<string> PhotosUrls { get; }
    
}