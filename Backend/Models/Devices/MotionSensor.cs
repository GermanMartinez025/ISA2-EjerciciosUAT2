using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelInterface.Notifications;

namespace Models.Devices;

public class MotionSensor : AMotionSensor
{
    
    public MotionSensor()
    {
    }
    
    public MotionSensor(string name, string modelNumber, string description, string photos, ACompany company) : this(name, modelNumber, description, new List<string> {photos}, company)
    {
    }
    
    public MotionSensor(string name, string modelNumber, string description, List<string> photos, ACompany company) : base(name, modelNumber, description, photos, company)
    {
    }
    
    public MotionSensor(string name, string modelNumber, string description, List<string> photos, ACompany company, string mainPhoto) : base(name, modelNumber, description, photos, company, mainPhoto)
    {
    }


   
}