using ModelInterface.Companys;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class ASensor : ADevice
{

    public override DeviceEnum DeviceType { get; set; } = DeviceEnum.Sensor;
    public override bool CanRepeatLastEvent { get; } = false;
    protected ASensor() : base()
    {
    }
    
    public ASensor (string name, string modelNumber, string description, List<string> photos, ACompany company, string mainPhoto) : base(name, modelNumber, description, photos, company, mainPhoto)
    { 
    }
    
    public ASensor (string name, string modelNumber, string description, List<string> photos, ACompany company) : base(name, modelNumber, description, photos, company)
    { 
    }


    public override bool SupportEvent(EventType eventType)
    {
        if (eventType == EventType.StatusOpen)
        {
            return true;
        }
        if (eventType == EventType.StatusClose)
        {
            return true;
        }
        
        return false;
    }
}