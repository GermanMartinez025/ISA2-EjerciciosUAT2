using ModelInterface.Devices;
using Moq;

namespace ModelsTests.Devices;

[TestClass]
public class DevicePhotoTests
{
    [TestMethod]
    public void ParametrizedDevicePhotoConstructor_WhenCalled_CreatesNewDevicePhoto()
    {
        var url = "photo.jpg";
        
        var devicePhoto = new DevicePhoto(url) {Id = 1, Device = new Mock<ADevice>().Object, DeviceId = 1};
        
        Assert.AreEqual(devicePhoto.Url, url);
        Assert.AreEqual(devicePhoto.Id, 1);
        Assert.IsNotNull(devicePhoto.Device);
        Assert.AreEqual(devicePhoto.DeviceId, 1);
    }

    [TestMethod]
    public void NoParametrizedDevicePhotoConstructor_WhenCalled_CreatesNewDevicePhoto()
    {
        var devicePhoto = new DevicePhoto();
        
        Assert.IsNotNull(devicePhoto);
    }
}