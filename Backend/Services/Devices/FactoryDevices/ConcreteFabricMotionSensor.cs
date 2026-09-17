using ModelInterface.Companys;
using ModelInterface.Devices;
using Models.Devices;
using ModelsAPI.Devices;

namespace Services.Devices;

public class ConcreteFabricMotionSensor : FabricDevice
{
    protected override ADevice CreateConcreteDevice(RequestCreateDevice request, ACompany company)
    {
        return 
            request.MainPhoto != null 
                ? new MotionSensor(request.Name, request.ModelNumber, request.Description, request.Photos, company, request.MainPhoto)
                : new MotionSensor(request.Name, request.ModelNumber, request.Description, request.Photos, company);
    }
}