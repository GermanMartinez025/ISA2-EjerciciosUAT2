using ModelException;
using ModelInterface.Companys;

namespace ModelInterface.Users.UserType;

public abstract class ACompanyOwner : AUserType
{
    public override RolesEnum Role { get; } = RolesEnum.CompanyOwner;
    public virtual ACompany? Company { get; set; }
    public virtual int? CompanyId { get; set; }
    public virtual bool HasCompanyAssigned => Company != null;
    
    protected ACompanyOwner() {}

    protected ACompanyOwner(AUser user) : base(user) {}

    public void AssingOwnerAndAddCompany(ACompany company)
    {
        if (HasCompanyAssigned)
        {
            throw new ConflictException("User already has a company assigned.");
        }
        Company = company ?? throw new NotFoundException("The company cannot be null.");
        CompanyId = company.Id;
    }
}