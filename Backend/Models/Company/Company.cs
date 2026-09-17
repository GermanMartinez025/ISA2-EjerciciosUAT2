using ModelInterface.Companys;
using Models.Users.UserTypes;

namespace Models.Company;

public class Company : ACompany
{
    public Company()
    {
    }

    public Company(string rut, string name, string logoType, CompanyOwner companyOwner, string validationType) : base(rut, name, logoType, companyOwner, validationType)
    {
    }
}