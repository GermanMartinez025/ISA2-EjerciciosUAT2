using ModelInterface.Companys;

namespace ModelsAPI.Companies;

public class ResponseAddCompanyToOwner
{
    public int Id { get; set; }
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoType { get; set; }
    
    public ResponseAddCompanyToOwner()
    {
    }

    public ResponseAddCompanyToOwner(ICompany company)
    {
        Id = company.Id;
        Rut = company.Rut;
        Name = company.Name;
        LogoType = company.LogoType;
    }

}