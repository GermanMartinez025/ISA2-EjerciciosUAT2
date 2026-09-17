using ModelException;
using ModelInterface.Companys;
using ModelInterface.Notifications;

namespace ModelInterface.Devices;

public abstract class ADevice : IDevice
{
    public virtual int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<DevicePhoto> Photos { get; set; } = new();
    public ACompany Company { get; set; }
    public abstract DeviceEnum DeviceType { get; set; }
    public int CompanyId { get; set; }
    public abstract bool CanRepeatLastEvent { get; }
    public string? MainPhoto { get; set; }

    protected ADevice()
    {
    }
    
    protected ADevice(string name, string modelNumber, string description, List<string> photos, ACompany company) : this(name, modelNumber, description, photos, company, photos[0])
    {
    }
    
    protected ADevice(string name, string modelNumber, string description, List<string> photos, ACompany company, string mainPhoto)
    {
        Validate(name, modelNumber, description, photos, company, mainPhoto);
        Name = name;
        ModelNumber = modelNumber;
        Description = description;
        Photos = photos.Select(photo => new DevicePhoto(photo)).ToList();
        Company = company;
        CompanyId = company.Id;
        MainPhoto = mainPhoto;
    }
    
    public abstract bool SupportEvent(EventType eventType);
    
    private void Validate(string name, string modelNumber, string description, List<string> photos, ACompany? company, string mainPhoto)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Name is required.");

        if (modelNumber == "" || modelNumber == null)
            throw new BadRequestException("ModelNumber is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new BadRequestException("Description is required.");

        if (photos == null || photos.Count == 0)
            throw new BadRequestException("Photos URL is required.");
        
        if (string.IsNullOrWhiteSpace(mainPhoto))
            throw new BadRequestException("MainPhoto URL is required.");
        
        if (!photos.Contains(mainPhoto))
            throw new BadRequestException("MainPhoto URL is not in photos list.");
        
        if (company == null)
            throw new BadRequestException("Companys is required.");
    }
    
    public virtual List<string> PhotosUrls => Photos.Select(photo => photo.Url).ToList();

}