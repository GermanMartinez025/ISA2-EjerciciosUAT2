using ControllersInterfaces.Importers;
using ModelException;
using Models.Importers;
using ModelsAPI.Devices;
using ServicesInterfaces.Company;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Importers;
using ServicesInterfaces.Sessions;

namespace Controllers.Importers;

public class ImporterController : IImporterController
{
    private readonly IImporterService _importerService;
    private readonly ISessionService _sessionService;
    private readonly ICompanyService _companyService;
    private readonly IDeviceServices _deviceServices;

    public ImporterController(IImporterService importerService, ISessionService sessionService, ICompanyService companyService, IDeviceServices deviceServices)
    {
        _importerService = importerService;
        _sessionService = sessionService;
        _companyService = companyService;
        _deviceServices = deviceServices;
    }
 

    public List<string> GetAllImporters()
    {
        return _importerService.LoadImporters();
    }

    public List<ImportedDevice> Import(string importerName, string sourcePath, int companyId, string token)
    {
        var companyOwnerId = _sessionService.GetUserIdFromSession(token);
        var company = _companyService.GetCompany(companyId);
        
        _companyService.ValidateCompanyOwner(companyOwnerId, company);
        
        var importedDevices = _importerService.Import(importerName, sourcePath);

        foreach (var importedDevice in importedDevices)
        {
            var createDeviceRequest = MapToCreateDeviceRequest(importedDevice);
            _deviceServices.AddDevice(createDeviceRequest, company);
        }

        return importedDevices;
    }
    
    private RequestCreateDevice MapToCreateDeviceRequest(ImportedDevice importedDevice)
    {
        return new RequestCreateDevice
        {
            Name = importedDevice.Name,
            ModelNumber = importedDevice.Model,
            Description = "No description.",
            Type = importedDevice.Type.ToLower() switch
            {
                "camera" => "Camera",
                "sensor-open-close" => "Sensor",
                "sensor-movement" => "MotionSensor",
                "smartlamp" => "SmartLamp",
                _ => throw new BadRequestException($"Unknown device type: {importedDevice.Type}")
            },
            Photos = importedDevice.Photos != null
                ? importedDevice.Photos.Select(photo => photo.Path).Where(path => !string.IsNullOrWhiteSpace(path)).ToList()
                : new List<string>(),
            MainPhoto = importedDevice.MainPhoto,
            MovementDetection = importedDevice.MovementDetection,
            PersonDetection = importedDevice.PersonDetection,
            OutsideEnvironment = importedDevice.OutsideEnvironment
        };
    }

}