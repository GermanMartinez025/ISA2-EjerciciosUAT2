using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Users;
using Models.Users.UserTypes;
using Moq;
using Repositories.UserRepositories;

namespace RepositoryTests.Users;

[TestClass]
public class CompanyOwnerRepositoryTests
{
    private Mock<CompanyOwner> _companyOwnerMock;
    private CompanyOwner _companyOwner;
    private IQueryable<ACompanyOwner> _data;
    private Mock<DbSet<ACompanyOwner>> _mockSet;
    private Mock<DbContext> _dbContext;
    private CompanyOwnerRepository _companyOwnerRepository;
    

    [TestInitialize]
    public void TestInitialize()
    {
        _companyOwnerMock = new Mock<CompanyOwner>();
        _companyOwnerMock.Setup(x => x.User).Returns(new User());
        _companyOwnerMock.Setup(x => x.UserId).Returns(1);
        _companyOwner = _companyOwnerMock.Object;
        
        _data = new List<CompanyOwner>
        {
            _companyOwner
        }.AsQueryable();
        
        _mockSet = new Mock<DbSet<ACompanyOwner>>();
        
        _mockSet.As<IQueryable<ACompanyOwner>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<ACompanyOwner>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<ACompanyOwner>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<ACompanyOwner>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<ACompanyOwner>()).Returns(_mockSet.Object);
        _companyOwnerRepository = new CompanyOwnerRepository(_dbContext.Object);
        
    }
    
    [TestMethod]
    public void GetById_ShouldReturnCompanyOwner()
    {
        var result = _companyOwnerRepository.GetById(1);
        
        Assert.AreEqual(_companyOwner, result);
    }
    
    [TestMethod]
    public void GetAll_ShouldReturnAllCompanyOwners()
    {
        var result = _companyOwnerRepository.GetAll();
        
        Assert.AreEqual(_data.Count(), result.Count);
    }
    
    [TestMethod]
    public void GetById_ShouldThrowException_WhenCompanyOwnerNotFound()
    {
        Assert.IsNull(_companyOwnerRepository.GetById(2));
    }

    [TestMethod]
    public void UpdateCompanyOwner_ShouldUpdateCompanyOwner()
    {
        var user = _companyOwner.User;
        user.FirstName = "Jane";
        user.LastName = "Doe";
        user.Email = "jhon@mail.com";
        user.Password = "newPassword123@";
        
        _companyOwnerRepository.Update(_companyOwner);
        
        _mockSet.Verify(m => m.Update(It.IsAny<CompanyOwner>()), Times.Once());
    }
}