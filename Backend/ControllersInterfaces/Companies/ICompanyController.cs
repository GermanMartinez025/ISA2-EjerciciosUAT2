using ModelsAPI.Companies;

namespace ControllersInterfaces.Companies;

public interface ICompanyController
{
    public ResponseGetComapanies GetCompanies(int page, int pageSize, List<string>? companyNames, List<string>? ownerFullNames);
    public ResponseAddCompanyToOwner AddOneCompanyToOwner(string token, RequestAddCompanyToOwner request);
    ResponseCompany GetMyCompany(string token);
    
    ResponseCompany UpdateCompany(string token, int companyId, string newValidationType);
}