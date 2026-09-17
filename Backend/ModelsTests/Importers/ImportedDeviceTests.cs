using Models.Importers;

namespace ModelsTests.Importers;

[TestClass]
public class ImportedDeviceTests
{
    [TestMethod]
    public void ImportedDeviceConstructor_ShouldCreateInstance()
    {
        string id = "1";
        string name = "Device";
        string model = "Model";
        string type = "Type";
        List<Photo> photos = new List<Photo>();
        
        bool? personDetection = true;
        bool? movementDetection = false;
        
        ImportedDevice importedDevice = new ImportedDevice(id, type, name, model, photos, personDetection, movementDetection);
        
        Assert.AreEqual(id, importedDevice.Id);
    }
    
    [TestMethod]
    public void ImportedDeviceConstructor_SetMainPhoto()
    {
        string id = "1";
        string name = "Device";
        string model = "Model";
        string type = "Type";
        List<Photo> photos = new List<Photo>
        {
            new Photo("photo1.jpg", true),
            new Photo("photo2.jpg", false)
        };
        
        bool? personDetection = true;
        bool? movementDetection = false;
        
        ImportedDevice importedDevice = new ImportedDevice(id, type, name, model, photos, personDetection, movementDetection);
        
        Assert.AreEqual("photo1.jpg", importedDevice.MainPhoto);
    }
    
}