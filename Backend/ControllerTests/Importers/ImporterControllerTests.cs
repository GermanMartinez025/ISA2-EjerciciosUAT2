using Controllers.Importers;
using Models.Company;
using Models.Importers;
using ModelsAPI.Devices;
using Moq;
using ServicesInterfaces.Company;
using ServicesInterfaces.Devices;
using ServicesInterfaces.Importers;
using ServicesInterfaces.Sessions;

namespace ControllerTests.Importers;

[TestClass]
public class ImporterControllerTests
{
    private Mock<IImporterService> _mockImporterService;
    private Mock<ISessionService> _mockSessionService;
    private Mock<ICompanyService> _mockCompanyService;
    private Mock<IDeviceServices> _deviceServices;
    private ImporterController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _mockImporterService = new Mock<IImporterService>();
        _mockSessionService = new Mock<ISessionService>();
        _mockCompanyService = new Mock<ICompanyService>();
        _deviceServices = new Mock<IDeviceServices>();
        _controller = new ImporterController(_mockImporterService.Object, _mockSessionService.Object,
            _mockCompanyService.Object, _deviceServices.Object);
    }

    [TestMethod]
    public void GetAllImporters_ShouldReturnListOfImporters()
    {
        var expectedImporters = new List<string> { "JsonImporter" };
        _mockImporterService.Setup(s => s.LoadImporters()).Returns(expectedImporters);

        var result = _controller.GetAllImporters();

        Assert.IsNotNull(result);
        CollectionAssert.AreEqual(expectedImporters, result);
    }

    [TestMethod]
    public void ExecuteImport_ShouldReturnListOfImportedDevices()
    {
        var importerName = "JsonImporter";
        var sourcePath = "path/to/devices.json";
        var companyId = 1;
        var token = "token";

        var expectedDevices = new List<ImportedDevice>
        {
            new ImportedDevice("id1", "camera", "name1", "model1", new List<Photo>(), true, false),
            new ImportedDevice("id2", "camera", "name2", "model2", new List<Photo>(), false, true)
        };

        _mockImporterService.Setup(s => s.Import(importerName, sourcePath)).Returns(expectedDevices);

        var result = _controller.Import(importerName, sourcePath, companyId, token);

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedDevices.Count, result.Count);
        Assert.AreEqual(expectedDevices[0].Id, result[0].Id);
        Assert.AreEqual(expectedDevices[1].Name, result[1].Name);
    }

    [TestMethod]
    public void ExecuteImport_ShouldCallAddDeviceForEachImportedDevice()
    {
        var importerName = "JsonImporter";
        var sourcePath = "path/to/devices.json";
        var companyId = 1;
        var token = "token";

        var expectedDevices = new List<ImportedDevice>
        {
            new ImportedDevice("id1", "camera", "name1", "model1", new List<Photo>(), true, false),
            new ImportedDevice("id2", "camera", "name2", "model2", new List<Photo>(), false, true)
        };

        _mockImporterService.Setup(s => s.Import(importerName, sourcePath)).Returns(expectedDevices);

        var result = _controller.Import(importerName, sourcePath, companyId, token);

        _deviceServices.Verify(s => s.AddDevice(It.IsAny<RequestCreateDevice>(), It.IsAny<Company>()),
            Times.Exactly(expectedDevices.Count));
    }
}