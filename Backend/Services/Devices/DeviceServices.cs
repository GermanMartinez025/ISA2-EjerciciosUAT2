using IRepositories.Repositories.DeviceRepositories;
using ModelException;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModeloValidador.Abstracciones;
using ModelsAPI.Devices;
using Services.Validations;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Validations;

namespace Services.Devices;

public class DeviceServices : IDeviceServices
{
    private IDeviceRepository _deviceRepository;
    private readonly IValidationProvider _validationProvider;


    public DeviceServices(IDeviceRepository deviceRepository, IValidationProvider validationProvider)
    {
        _deviceRepository = deviceRepository;
        _validationProvider = validationProvider;
    }


    public ADevice AddDevice(RequestCreateDevice request, ACompany company)
    {
        if (!string.IsNullOrEmpty(company.ValidationType))
        {
            ValidateDeviceModel(request.ModelNumber, company);
        }            
        
        var device = FabricDevice.CreateDevice(request, company);
        
        return _deviceRepository.Create(device);
    }
    
    public ADevice GetDevice(int deviceId)
    {
        ADevice device = _deviceRepository.GetById(deviceId);

        ValidateDeviceNotNull(device);

        return device;
    }
    
    public List<IDevice> GetFilterDevices(int page, int pageSize, Dictionary<string, object>? filters)
    {
        return _deviceRepository.GetPaginated(page, pageSize, filters)
            .Select(device => (IDevice)device)
            .ToList();
    }
    public int GetAmountOfDevices(Dictionary<string, object>? filters)
    {
        return _deviceRepository.Count(filters);
    }
    
    private void ValidateDeviceNotNull(ADevice? device)
    {
        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }
    }
    
    private void ValidateDeviceModel(string model, ACompany company)
    {
        var validator = _validationProvider.GetValidator(company.ValidationType);

        if (!validator.EsValido(new Modelo { Value = model }))
        {
            throw new BadRequestException($"Device model '{model}' is invalid for validation type '{company.ValidationType}'.");
        }
    }
}