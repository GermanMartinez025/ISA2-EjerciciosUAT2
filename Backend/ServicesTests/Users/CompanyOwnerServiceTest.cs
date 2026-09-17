using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Company;
using Models.Users.UserTypes;
using Moq;
using Services.Users;

namespace ServicesTests.Users;

[TestClass]
public class CompanyOwnerServiceTest
{
    private Mock<ICompanyOwnerRepository> _companyOwnerRepositoryMock;
    private CompanyOwnerService _companyOwnerService;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _companyOwnerRepositoryMock = new Mock<ICompanyOwnerRepository>();
        _companyOwnerService = new CompanyOwnerService(_companyOwnerRepositoryMock.Object);
    }

    [TestMethod]
    public void GetCompanyOwner_ById_ReturnsCompanyOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner
        (
            "John",
            "Doe",
            "jhon@mail.com",
            "password1A23@"
        );
        
        _companyOwnerRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(companyOwner);
        
        var result = _companyOwnerService.GetById(1);
        
        Assert.IsInstanceOfType(result, typeof(CompanyOwner));
        _companyOwnerRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public void AddCompanyToCompanyOwner_WhenShouldCompanyOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner
        ( "John", "Doe","jhon@mail.com","password1A23@" );
        
        Company company = new Company
        {
            Id = 1,
            Name = "Companys",
            Rut = "123456789",
            LogoType = "png"
        };
        
        _companyOwnerRepositoryMock.Setup(x => x.Update(It.IsAny<CompanyOwner>()));
        
        _companyOwnerService.AddCompanyToCompanyOwner(companyOwner, company);
        
        _companyOwnerRepositoryMock.Verify(x => x.Update(It.IsAny<CompanyOwner>()), Times.Once);
    }

    [TestMethod]
    public void ValidateAssignedCompany_ThrowsExceptionWhenOwnerHasNoCompanyAssigned()
    {
        Mock<ACompanyOwner> companyOwner = new Mock<ACompanyOwner>();
        companyOwner.Setup(x => x.HasCompanyAssigned).Returns(false);
        
        Assert.ThrowsException<ConflictException>(() => _companyOwnerService.ValidateAssignedCompany(companyOwner.Object));
        
    }

    [TestMethod]
    public void ValidateAssignedCompany_DoesNotThrowExceptionWhenOwnerHasCompanyAssigned()
    {
        var companyOwner = new Mock<ACompanyOwner>();
        companyOwner.Setup(x => x.HasCompanyAssigned).Returns(true);
        
        try
        {
            _companyOwnerService.ValidateAssignedCompany(companyOwner.Object);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected no exception, but got: " + ex.Message);
        }
    }

    [TestMethod]
    public void GetCompanyOwner_ById_ThrowsExceptionWhenCompanyOwnerIsNull()
    {
        _companyOwnerRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((ACompanyOwner)null);
        
        var exception = Assert.ThrowsException<NotFoundException>(() => _companyOwnerService.GetById(1));
        
        Assert.AreEqual("Companys owner not found", exception.Message);
    }
    
}