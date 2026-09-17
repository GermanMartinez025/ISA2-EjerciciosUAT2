using ControllersInterfaces.Importers;
using Models.Importers;
using ModelsAPI.Importers;
using Moq;
using WebAPI.APIs;

namespace WebAPITests.Importers;

[TestClass]
public class ImporterApiTests
{
    private Mock<IImporterController> _mockImporterController;
    private ImporterAPI _api;
    
    [TestInitialize]
    public void SetUp()
    {
        _mockImporterController = new Mock<IImporterController>();
        _api = new ImporterAPI(_mockImporterController.Object);
    }

    [TestMethod]
    public void GetAvailableImporters_ShouldCallControllerMethod()
    {
        _mockImporterController.Setup(c => c.GetAllImporters()).Returns(new List<string>());

        var result = _api.GetAvailableImporters();

        _mockImporterController.Verify(c => c.GetAllImporters(), Times.Once);
    }

    

      
    
}