using ModelInterface.Companys;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class ACamera : ADevice
{
    public bool OutsideEnvironment { get; set; }
    public bool MovementDetection { get; set; }
    public bool PersonDetection { get; set; }
    
    public override bool CanRepeatLastEvent { get; } = true;

    protected ACamera() : base()
    {
    }
    
    public ACamera (string name, string modelNumber, 
        string description, List<string> photos, ACompany company, string mainPhoto, bool outsideEnvironment, bool movementDetection, bool personDetection) : base(name, modelNumber, description, photos, company, mainPhoto)
    {
        OutsideEnvironment = outsideEnvironment;
        MovementDetection = movementDetection;
        PersonDetection = personDetection;
    }
    
    public ACamera (string name, string modelNumber, 
        string description, List<string> photos, ACompany company, bool outsideEnvironment, bool movementDetection, bool personDetection) : this(name, modelNumber, description, photos, company, photos[0],outsideEnvironment, movementDetection, personDetection)
    {
    }

    public override DeviceEnum DeviceType { get; set; } = DeviceEnum.Camera;

    public override bool SupportEvent(EventType eventType)
    {
        if (eventType == EventType.MovementDetection)
        {
            return MovementDetection;
        }
        if (eventType == EventType.PersonDetection)
        {
            return PersonDetection;
        }
        return false;
    }
    
}