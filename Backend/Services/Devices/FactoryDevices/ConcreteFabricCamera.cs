using ModelInterface.Companys;
using Models.Devices;
using ModelsAPI.Devices;

namespace Services.Devices;

public class ConcreteFabricCamera : FabricDevice
{
    protected override Camera CreateConcreteDevice(RequestCreateDevice request, ACompany company)
    {
        return 
            request.MainPhoto != null 
                ? new Camera(request.Name, request.ModelNumber, request.Description, request.Photos, company, request.MainPhoto, request.OutsideEnvironment.Value, request.MovementDetection.Value, request.PersonDetection.Value) 
                : new Camera(request.Name, request.ModelNumber, request.Description, request.Photos, company, request.OutsideEnvironment.Value, request.MovementDetection.Value, request.PersonDetection.Value);;
    }
}