namespace ModelsAPI.Companies;

public class RequestAddCompanyToOwner
{
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoType { get; set; }
    public string? ValidationType { get; set; }
}