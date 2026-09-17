using ModelInterface.Companys;
using Models.Devices;
using ModelsAPI.Devices;

namespace Services.Devices;

public class ConcreteFabricSensor : FabricDevice
{
    protected override Sensor CreateConcreteDevice(RequestCreateDevice request, ACompany company)
    {
      return 
          request.MainPhoto != null 
              ? new Sensor(request.Name, request.ModelNumber, request.Description, request.Photos, company, request.MainPhoto)
              : new Sensor(request.Name, request.ModelNumber, request.Description, request.Photos, company);;
    }
}