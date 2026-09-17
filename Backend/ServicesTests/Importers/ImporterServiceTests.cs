using Models.Importers;
using Services.Importers;

namespace ServicesTests.Importers;

[TestClass]
public class ImporterServiceTests
{
    private ImporterService _service;

    [TestInitialize]
    public void SetUp()
    {
        _service = new ImporterService("../../../../Importers");
    }
    
    [TestMethod]
    public void LoadImporters_ShouldReturnListOfImporters()
    {
        var expected = new List<string> { "JsonImporter" };
        
        var actual = _service.LoadImporters();
        
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [TestMethod]
    public void Import_ShouldReturnListOfImportedDevices()
    {
        var source = Path.Combine("../../../../","Importers/","device.json");
        var expected = new List<ImportedDevice> 
        { 
            new ImportedDevice("id", "type", "name", "model", new List<Photo>(), true, false) 
        };

        var actual = _service.Import("JsonImporter", source);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected.Count, actual.Count);
        Assert.AreEqual(expected[0].Id, actual[0].Id);
        Assert.AreEqual(expected[0].Type, actual[0].Type);
        Assert.AreEqual(expected[0].Name, actual[0].Name);
        Assert.AreEqual(expected[0].Model, actual[0].Model);
        Assert.AreEqual(expected[0].Photos.Count, actual[0].Photos.Count);
        Assert.AreEqual(expected[0].PersonDetection, actual[0].PersonDetection);
        Assert.AreEqual(expected[0].MovementDetection, actual[0].MovementDetection);
    }


}