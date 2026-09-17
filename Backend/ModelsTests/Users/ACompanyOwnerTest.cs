using ModelException;
using ModelInterface.Users;
using Models.Company;
using Models.Users;
using Models.Users.UserTypes;
using InvalidOperationException = System.InvalidOperationException;

namespace ModelsTests.Users;

[TestClass]
public class ACompanyOwnerTest
{
    private CompanyOwner companyOwner;
    private AUser authenticatedUser;
    private Company company;
    private string firstName;
    private string lastName;
    private string email;
    private string password;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        firstName = "John";
        lastName = "Doe";
        email = "john.doe@example.com";
        password = "password123@";
        companyOwner = new CompanyOwner(firstName, lastName, email, password);
        authenticatedUser = companyOwner.User;
        company = new Company { Id = 1, Name = "Test Company", Rut = "1234567890", LogoType = "TestLogo" };
    }
    
    [TestMethod]
    public void NoParametrizedCompanyUserConstructor_WhenCalled_CreatesNewHomeUser()
    {
        Assert.AreEqual(authenticatedUser.Roles[0].Role, RolesEnum.CompanyOwner);
        Assert.IsTrue(authenticatedUser.CreationDate < DateTime.Now);
    }
    
    [TestMethod]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        Assert.AreEqual(firstName, authenticatedUser.FirstName, "FirstName should be set correctly.");
        Assert.AreEqual(lastName, authenticatedUser.LastName, "LastName should be set correctly.");
        Assert.AreEqual(email, authenticatedUser.Email, "Email should be set correctly.");
        Assert.AreEqual(password, authenticatedUser.Password, "Password should be set correctly.");
        Assert.AreEqual(RolesEnum.CompanyOwner, authenticatedUser.Roles[0].Role, "Role should be set to CompanyOwner.");
    }
    
    [TestMethod]
    public void AssignACompany_OwnerAsAssignedFalseForDefault()
    {
        Assert.IsFalse(companyOwner.HasCompanyAssigned, "HasCompanyAssigned should be false before assigning a company.");
    }
    
    [TestMethod]
    public void AssingOwnerAndAddCompany_ShouldSetCompanyAndHasCompanyAssignedTrue_WhenNoCompanyIsAssigned()
    {
        Company company = new Company { Id = 1, Name = "Test Companys", Rut = "1234567890", LogoType = "TestLogo" };
        
        companyOwner.AssingOwnerAndAddCompany(company);
        
        Assert.IsTrue(companyOwner.HasCompanyAssigned, "The property HasCompanyAssigned should be set to true.");
        Assert.IsNotNull(companyOwner.Company, "The Companys property should not be null.");
        Assert.AreEqual(company, companyOwner.Company, "The Companys property should match the assigned company.");
    }
    
    [TestMethod]
    public void AssingOwnerAndAddCompany_ShouldThrowException_WhenOwnerAlreadyHasCompanyAssigned()
    {
        companyOwner.AssingOwnerAndAddCompany(company);

        Company newCompany = new Company { Id = 2, Name = "New Companys", Rut = "1234567890", LogoType = "NewLogo" };
    
        var exception = Assert.ThrowsException<ConflictException>(() => companyOwner.AssingOwnerAndAddCompany(newCompany));
        Assert.AreEqual("User already has a company assigned.", exception.Message);
    }
    
    [TestMethod]
    public void AssingOwnerAndAddCompany_ShouldThrowException_WhenCompanyIsNull()
    {
        var exception = Assert.ThrowsException<NotFoundException>(() => companyOwner.AssingOwnerAndAddCompany(null));
    
        Assert.AreEqual("The company cannot be null.", exception.Message);
    }
}