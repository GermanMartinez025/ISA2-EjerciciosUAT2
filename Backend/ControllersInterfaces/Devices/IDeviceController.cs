using ModelsAPI.Devices;

namespace ControllersInterfaces.Devices;

public interface IDeviceController
{
    public ResponseCreateDevice CreateDevice(RequestCreateDevice request, string token);
    public ResponseGetAllDevices GetFilterDevices(int page, int pageSize, List<string>? deviceName, List<string>? deviceModel, List<string>? companyName, List<string>? deviceType);
    public ResponseGetTypeDevice GetSupportedDevices();
}