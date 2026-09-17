using ModelInterface.Companys;
using ModelsAPI.Companies;

namespace ServicesInterfaces.Company;

public interface ICompanyService
{
    public ACompany? GetCompany(int id);
    public List<ACompany>? GetAllCompanys(); 
    public ACompany AddCompanyToOwner(int companyOwnerId, RequestAddCompanyToOwner request);
    public List<ACompany> GetCompanies(int page, int pageSize, Dictionary<string, object>? filters);
    int GetAmountOfCompanies(Dictionary<string, object>? filters);
    ICompany GetMyCompany(int companyOwnerId);
    
    ACompany UpdateCompany(int companyId,int companyOwnerId, string newValidationType);

    void ValidateCompanyOwner(int companyOwnerId, ACompany company);
}