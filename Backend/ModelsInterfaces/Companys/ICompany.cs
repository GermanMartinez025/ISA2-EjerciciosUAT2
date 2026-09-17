using ModelInterface.Devices;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelInterface.Companys;

public interface ICompany
{
    public int Id { get; set; }
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoType { get; set; }
    public ACompanyOwner CompanyOwner { get; set; }
    public List<ADevice> Devices { get; set; }
    
    public string ValidationType { get; set; }
    
}