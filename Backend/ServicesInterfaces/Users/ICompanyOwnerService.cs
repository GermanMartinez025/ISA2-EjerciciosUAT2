using ModelInterface.Companys;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users.UserTypes;

namespace ServicesInterfaces.Users;

public interface ICompanyOwnerService
{
    public ACompanyOwner GetById(int companyOwnerId);
    public  void AddCompanyToCompanyOwner(ACompanyOwner companyOwner, ACompany company);
    void ValidateAssignedCompany(ACompanyOwner companyOwner);
}