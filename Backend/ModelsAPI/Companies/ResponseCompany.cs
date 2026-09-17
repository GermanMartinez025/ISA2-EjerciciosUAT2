using ModelInterface.Companys;

namespace ModelsAPI.Companies;

public class ResponseCompany
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string OwnerFullName { get; set; }
    public string OwnerEmail { get; set; }
    public string CompanyRut { get; set; }
    public string CompanyLogoType { get; set; }
    public int TotalDevices { get; set; }
    
    public ResponseCompany()
    {
        
    }
    
    public ResponseCompany(ICompany company)
    {
        Id = company.Id;
        CompanyName = company.Name;
        OwnerFullName = $"{company.CompanyOwner}";
        OwnerEmail = company.CompanyOwner.Email;
        CompanyRut = company.Rut;
        CompanyLogoType = company.LogoType;
        TotalDevices = company.Devices.Count;
    }
}