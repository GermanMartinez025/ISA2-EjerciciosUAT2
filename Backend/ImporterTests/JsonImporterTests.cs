using Importer;

namespace ImporterTests;

[TestClass]
public class JsonImporterTests
{
    [TestMethod]
public void Import_ValidJson_ReturnsImportedDevices()
{
    var importer = new JsonImporter();
    var jsonFilePath = "testDevices.json";

    File.WriteAllText(jsonFilePath, @"{
        ""dispositivos"": [
            {
                ""id"": ""123"",
                ""tipo"": ""camera"",
                ""nombre"": ""Test Device"",
                ""modelo"": ""T123"",
                ""fotos"": [
                    { ""path"": ""https://example.com/photo1.jpg"", ""es_Principal"": true },
                    { ""path"": ""https://example.com/photo2.jpg"", ""es_Principal"": false }
                ],
                ""person_Detection"": true,
                ""movement_Detection"": false
            }
        ]
    }");

    var devices = importer.Import(jsonFilePath);

    Assert.IsNotNull(devices);
    Assert.AreEqual(1, devices.Count);

    var device = devices[0];
    Assert.AreEqual("123", device.Id);
    Assert.AreEqual("camera", device.Type);
    Assert.AreEqual("Test Device", device.Name);
    Assert.AreEqual("T123", device.Model);
    Assert.AreEqual(2, device.Photos.Count);
    Assert.IsTrue(device.PersonDetection);
    Assert.IsFalse(device.MovementDetection);

    var firstPhoto = device.Photos[0];
    Assert.AreEqual("https://example.com/photo1.jpg", firstPhoto.Path);
    Assert.IsTrue(firstPhoto.IsPrincipal);

    var secondPhoto = device.Photos[1];
    Assert.AreEqual("https://example.com/photo2.jpg", secondPhoto.Path);
    Assert.IsFalse(secondPhoto.IsPrincipal);
}

    
}