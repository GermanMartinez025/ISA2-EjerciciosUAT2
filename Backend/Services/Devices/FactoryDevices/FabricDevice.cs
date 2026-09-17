using ModelException;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelsAPI.Devices;

namespace Services.Devices;

public abstract class FabricDevice
{
    protected abstract ADevice CreateConcreteDevice(RequestCreateDevice request, ACompany company);
    
    private static FabricDevice GetFactoryDevice(RequestCreateDevice request)
    {
        return request.Type switch
        {
            "Camera" => new ConcreteFabricCamera(),
            "Sensor" => new ConcreteFabricSensor(),
            "MotionSensor" => new ConcreteFabricMotionSensor(),
            "SmartLamp" => new ConcreteFabricSmartLamp(),
            _ => throw new BadRequestException("Invalid type of device")
        };
    }
    
    public static ADevice CreateDevice(RequestCreateDevice requestAddUser, ACompany company)
    {
        var fabricUser = GetFactoryDevice(requestAddUser);
        return fabricUser.CreateConcreteDevice(requestAddUser, company);
    }
}