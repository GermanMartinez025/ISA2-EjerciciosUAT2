using ModelInterface.Devices;
using ModelInterface.Homes;

namespace ModelsAPI.Devices;

public class ResponseHomeDevice
{
    public Guid HardwareId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public List<string> Photos { get; set; }
    public StateType State { get; set; }
    public int HomeId { get; set; }
    public string MainPhoto { get; set; }

    public ResponseHomeDevice()
    {
    }

    public ResponseHomeDevice(AHomeDevice aHomeDevice)
    {
        HardwareId = aHomeDevice.HardwareId;
        Id = aHomeDevice.Device.Id;
        HomeId = aHomeDevice.HomeId;
        Name = aHomeDevice.Name;
        ModelNumber = aHomeDevice.Device.ModelNumber;
        Photos = aHomeDevice.Device.PhotosUrls;
        MainPhoto = aHomeDevice.Device.MainPhoto;
        State = aHomeDevice.Online ? StateType.Online : StateType.Offline;
    }
}