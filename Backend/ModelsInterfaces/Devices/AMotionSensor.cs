using ModelInterface.Companys;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class AMotionSensor : ADevice
{
    public override DeviceEnum DeviceType { get; set; } = DeviceEnum.MotionSensor;
    public override bool CanRepeatLastEvent { get; } = true;
    
    protected AMotionSensor() : base()
    {
    }
    public AMotionSensor(string name, string modelNumber, string description, List<string> photos, ACompany company, string mainPhoto) : base(name, modelNumber, description, photos, company, mainPhoto)
    {
    }
    
    public AMotionSensor(string name, string modelNumber, string description, List<string> photos, ACompany company) : base(name, modelNumber, description, photos, company)
    {
    }
    
    public override bool SupportEvent(EventType eventType)
    {
        return eventType == EventType.MovementDetection;
    }
}