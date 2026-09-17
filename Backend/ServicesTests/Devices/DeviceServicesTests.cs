using IRepositories.Repositories.DeviceRepositories;
using ModelException;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModeloValidador.Abstracciones;
using ModelsAPI.Devices;
using ModelValidation;
using Moq;
using Services.Devices;
using Services.Validations;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Validations;

namespace ServicesTests.Devices;

[TestClass]
public class DeviceServicesTests
{
    private Mock<IDeviceRepository> _deviceRepository;
    private Mock<ACompany> _company;
    private ADevice _device;
    private IDeviceServices _deviceServices;
    private Mock<IValidationProvider> _validationProvider;
    private ModelValidatorBasic _modelValidator;

    [TestInitialize]
    public void TestInitialize()
    {
        _company = new Mock<ACompany>();
        _deviceRepository = new Mock<IDeviceRepository>();
        _validationProvider = new Mock<IValidationProvider>();
        _deviceServices = new DeviceServices(_deviceRepository.Object, _validationProvider.Object);
        _modelValidator = new ModelValidatorBasic();
    }


    [TestMethod]
    public void ParametrizedDeviceFactoryConstructor_WhenCalled_CreatesNewDeviceFactory()
    {
        var deviceFactory = new DeviceServices(_deviceRepository.Object, _validationProvider.Object);
        
        Assert.IsNotNull(deviceFactory);
    }

    [TestMethod]
    public void CreateDevice_WhenCalledWithACamera_ShouldUseCameraService()
    {

        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Camera",
            ModelNumber = "ABCDEF",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "Camera",
            OutsideEnvironment = true,
            MovementDetection = true,
            PersonDetection = true
        };
        _validationProvider.Setup(v => v.GetValidator(It.IsAny<string>()))
            .Returns(_modelValidator);
        
        _company.Setup(c => c.ValidationType).Returns("ModelValidatorBasic");

        
        _deviceServices.AddDevice(request, _company.Object);

        _deviceRepository.Verify(s => s.Create(It.IsAny<ACamera>()), Times.Once);
    }

    [TestMethod]
    public void CreateDevice_WhenCalledWithASensor_ShouldUseSensorService()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Sensor",
            ModelNumber = "ABCDEF",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "Sensor",

        };
        _validationProvider.Setup(v => v.GetValidator(It.IsAny<string>()))
            .Returns(_modelValidator);
        
        _company.Setup(c => c.ValidationType).Returns("ModelValidatorBasic");

        _deviceServices.AddDevice(request, _company.Object);

        _deviceRepository.Verify(s => s.Create(It.IsAny<ASensor>()), Times.Once);
    }

    [TestMethod]
    public void CreateDevice_WhenCalledWithNoAvailableDevice_ShouldThrowException()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "ABCDEF",
            ModelNumber = "ABCDEF",
            Description = "Description",
            Photos = new List<string>() {"Photos"},
            Type = "Porter",
            OutsideEnvironment = true,
            MovementDetection = true,
            PersonDetection = true
        };
        _validationProvider.Setup(v => v.GetValidator(It.IsAny<string>()))
            .Returns(_modelValidator);
        
        _company.Setup(c => c.ValidationType).Returns("ModelValidatorBasic");

        Assert.ThrowsException<BadRequestException>(() => _deviceServices.AddDevice(request, _company.Object));

    }

    [TestMethod]
    public void GetDevice_WhenCalled_ShouldReturnDevice()
    {
        int deviceId = 1;
        var device = new Mock<ADevice>().Object;

        _deviceRepository.Setup(x => x.GetById(deviceId)).Returns(device);

        var result = _deviceServices.GetDevice(deviceId);

        Assert.AreEqual(device, result);
    }

    [TestMethod]
    public void GetDevice_WhenDeviceIsNull_ShouldThrowArgumentNullException()
    {
        int deviceId = 1;
        _deviceRepository.Setup(x => x.GetById(deviceId)).Returns((ADevice)null); // Simula un dispositivo no encontrado

        Assert.ThrowsException<NotFoundException>(() => _deviceServices.GetDevice(deviceId));
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenNoDevicesFound_ShouldReturnEmptyList()
    {
        int page = 1;
        int pageSize = 10;
        var filters = new Dictionary<string, object>
        {
            { "Type", "NonExistentType" }
        };
        var devices = new List<IDevice>(); // No hay dispositivos

        _deviceRepository.Setup(x => x.GetPaginated(page, pageSize, filters)).Returns(devices);
        
        var result = _deviceServices.GetFilterDevices(page, pageSize, filters);
        
        Assert.AreEqual(0, result.Count);
        _deviceRepository.Verify(x => x.GetPaginated(page, pageSize, filters), Times.Once);
    }
    
    [TestMethod]
    public void GetFilterDevices_WhenCalledWithFilters_ShouldReturnFilteredDevices()
    {
        int page = 1;
        int pageSize = 10;
        var filters = new Dictionary<string, object>
        {
            { "Type", "Camera" }
        };
        var mockDevice = new Mock<IDevice>();
        var devices = new List<IDevice> { mockDevice.Object };

        _deviceRepository.Setup(x => x.GetPaginated(page, pageSize, filters)).Returns(devices);
        
        var result = _deviceServices.GetFilterDevices(page, pageSize, filters);
        
        CollectionAssert.AreEqual(devices, result); 
        _deviceRepository.Verify(x => x.GetPaginated(page, pageSize, filters), Times.Once);
    }
    
    [TestMethod]
    public void GetAmountOfDevices_WhenCalledWithFilters_ShouldReturnFilteredCount()
    {
        var filters = new Dictionary<string, object>
        {
            { "Type", "Camera" }
        };
        
        _deviceRepository.Setup(x => x.Count(filters)).Returns(5);
        
        var result = _deviceServices.GetAmountOfDevices(filters);
        
        Assert.AreEqual(5, result); 
        _deviceRepository.Verify(x => x.Count(filters), Times.Once); 
    }

    
    [TestMethod]
    public void AddDevice_WhenModelIsValid_ShouldCreateDevice()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Camera",
            ModelNumber = "ABCDEF",
            Type = "Camera",
            OutsideEnvironment = true,
            MovementDetection = true,
            PersonDetection = true,
            Description = "Description",
            Photos = new List<string>() {"Photos"},
        };

        _validationProvider.Setup(v => v.GetValidator(It.IsAny<string>()))
            .Returns(_modelValidator);

        _company.Setup(c => c.ValidationType).Returns("ModelValidatorBasic");

        _deviceServices.AddDevice(request, _company.Object);

        _deviceRepository.Verify(s => s.Create(It.IsAny<ACamera>()), Times.Once);
    }
    
    [TestMethod]
    public void AddDevice_WhenModelIsInvalid_ShouldThrowException()
    {
        var request = new RequestCreateDevice
        {
            Id = 1,
            Name = "Camera",
            ModelNumber = "ABCDEFa",
            Type = "Camera",
            OutsideEnvironment = true,
            MovementDetection = true,
            PersonDetection = true,
            Description = "Description",
            Photos = new List<string>() {"Photos"},
        };

        _validationProvider.Setup(v => v.GetValidator(It.IsAny<string>()))
            .Returns(_modelValidator);

        _company.Setup(c => c.ValidationType).Returns("ModelValidatorBasic");
        
        Assert.ThrowsException<BadRequestException>(() => _deviceServices.AddDevice(request, _company.Object));
    }

    
}