using ModelInterface.Devices;
using ModelInterface.Homes;
using Models.Homes;

namespace Models.Devices;

public class HomeDevice : AHomeDevice
{
    
    public HomeDevice()
    {
    }
    
    public HomeDevice(ADevice device, AHome home) : base(device, home)
    {
    }
    
}