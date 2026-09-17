using ModelInterface.Companys;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class ASmartLamp : ADevice
{
    public override DeviceEnum DeviceType { get; set; } = DeviceEnum.SmartLamp;
    public override bool CanRepeatLastEvent { get; } = false;

    protected ASmartLamp() 
    {
    }
    
    public ASmartLamp(string name, string modelNumber, string description, List<string> photos, ACompany company, string mainPhoto) : base(name, modelNumber, description, photos, company, mainPhoto)
    {
    }
    
    public ASmartLamp(string name, string modelNumber, string description, List<string> photos, ACompany company) : base(name, modelNumber, description, photos, company)
    {
    }
    
   
    public override bool SupportEvent(EventType eventType)
    {
        return eventType is EventType.TurnOn or EventType.TurnOff;
    }
}