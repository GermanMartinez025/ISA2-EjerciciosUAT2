using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelsAPI.Devices;

namespace ServicesInterfaces.Devices;

public interface IDeviceServices
{
    public ADevice AddDevice(RequestCreateDevice request, ACompany company);
    public ADevice GetDevice(int deviceId);
    public List<IDevice> GetFilterDevices(int page, int pageSize, Dictionary<string, object>? filters);
    public int GetAmountOfDevices(Dictionary<string, object>? filters);
    public static  List<string> GetSupportedDevices()
    {
        return Enum.GetValues(typeof(DeviceEnum))
            .Cast<DeviceEnum>()
            .Select(e => e.ToString())
            .ToList();
    }
}