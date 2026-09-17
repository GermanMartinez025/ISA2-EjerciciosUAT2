using ModelInterface;
using ModelInterface.Companys;
using ModelInterface.Devices;

namespace Models.Devices;

public class Sensor : ASensor
{
    
    public Sensor()
    {
    }
    
    public Sensor(string name, string modelNumber,
        string description, string photos, ACompany company) : this(name, modelNumber, description, new List<string> {photos}, company)
    {
        
    }

    public Sensor(string name, string modelNumber,
        string description, List<string> photos, ACompany company) : base(name, modelNumber, description, photos, company)
    {
        
    }
    
    public Sensor(string name, string modelNumber,
        string description, List<string> photos, ACompany company, string mainPhoto) : base(name, modelNumber, description, photos, company, mainPhoto)
    {
        
    }
    
   
    
}