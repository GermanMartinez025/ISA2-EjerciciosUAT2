
using ModelInterface.Companys;
using ModelInterface.Devices;

namespace Models.Devices;

public class Camera : ACamera
{
    
    public Camera()
    {
    }
    
    public Camera(string name, string modelNumber,
        string description, string photos, ACompany company, bool outsideEnvironment, bool movementDetection,
        bool personDetection) : this(name, modelNumber, description, new List<string> {photos}, company, outsideEnvironment, movementDetection,
        personDetection)
    {
    }

    public Camera(string name, string modelNumber,
        string description, List<string> photos, ACompany company, bool outsideEnvironment, bool movementDetection,
        bool personDetection) : base(name, modelNumber, description, photos, company, outsideEnvironment, movementDetection,
        personDetection)
    {
    }
    
    public Camera(string name, string modelNumber,
        string description, List<string> photos, ACompany company, string mainPhoto, bool outsideEnvironment, bool movementDetection,
        bool personDetection) : base(name, modelNumber, description, photos, company, mainPhoto, outsideEnvironment, movementDetection,
        personDetection)
    {
        
    }
}