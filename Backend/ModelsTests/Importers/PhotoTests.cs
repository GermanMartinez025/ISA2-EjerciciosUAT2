using Models.Importers;

namespace ModelsTests.Importers;

[TestClass]
public class PhotoTests
{
    [TestMethod]
    public void PhotoConstructor_ShouldCreateInstance()
    {
        string path = "path";
        bool isPrincipal = true;
        
        Photo photo = new Photo(path, isPrincipal);
        
        Assert.AreEqual(path, photo.Path);
    }
    
}