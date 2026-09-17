using Microsoft.EntityFrameworkCore;
using ModelInterface.Users;
using Models.Users;
using Moq;
using Repositories.UserRepositories;

namespace RepositoryTests.Users;

[TestClass]
public class UserRepositoryTests
{
    private Mock<User> _aUser;
    private IQueryable<User> _data;
    private Mock<DbSet<User>> _mockSet;
    private Mock<DbContext> _dbContext;
    private UserRepository _userRepository;
    
    [TestInitialize]
    public void Setup()
    {
        _aUser = new Mock<User>();
        _aUser.Setup(u => u.Id).Returns(1);
        _aUser.Setup(u => u.HasRole(It.IsAny<string>())).Returns(true);
        _data = new List<User> { _aUser.Object }.AsQueryable();
        
        _mockSet = new Mock<DbSet<User>>();
        
        _mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
        
        
        _dbContext = new Mock<DbContext>();
        _dbContext.Setup(c => c.Set<User>()).Returns(_mockSet.Object);
        _userRepository = new UserRepository(_dbContext.Object);
    }


    [TestMethod]
    public void GetUserById_ShouldReturnUser()
    {
        var result = _userRepository.GetById(1);
        
        Assert.AreEqual(_aUser.Object, result);
    }
    
    [TestMethod]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        var result = _userRepository.GetAll();
        
        Assert.AreEqual(_data.ToList().Count, result.Count);
    }
    
    [TestMethod]
    public void GetUserById_WhenUserDoesNotExist_ShouldThrowException()
    {
        Assert.AreEqual(null,_userRepository.GetById(2));
    }
    
    [TestMethod]
    public void GetPaginatedUsers_ShouldReturnPaginatedUsers()
    {
        var result = _userRepository.GetPaginated(1, 1, null);
        
        Assert.AreEqual(1, result.Count);
    }
    
    [TestMethod]
    public void GetPaginatedUsers_WhenPageIsGreaterThanTotalPages_ShouldReturnEmptyList()
    {
        var result = _userRepository.GetPaginated(2, 1, null);
        
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void GetPaginatedUsers_WhenPageSizeIsGreaterThanTotalUsers_ShouldReturnAllUsers()
    {
        var result = _userRepository.GetPaginated(1, 2, null);
        
        Assert.AreEqual(_data.ToList().Count, result.Count);
    }
    
    [TestMethod]
    public void Count_ShouldReturnTotalUsers()
    {
        var result = _userRepository.Count();
        
        Assert.AreEqual(_data.ToList().Count, result);
    }
    
    [TestMethod]
    public void GetPaginatedUsers_WhenRolesAreSpecified_ShouldReturnUsersWithSpecifiedRoles()
    {
        _aUser.Setup(u => u.HasRole(It.Is((string s) => s == "CompanyOwner"))).Returns(false);
        var result = _userRepository.GetPaginated(1, 1, new Dictionary<string, object>() { { "role", new List<string>() { "CompanyOwner" } } });
        
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void Count_WhenRolesAreSpecified_ShouldReturnTotalUsersWithSpecifiedRoles()
    {
        _aUser.Setup(u => u.HasRole(It.Is((string s) => s == "CompanyOwner"))).Returns(false);
        var result = _userRepository.Count(new Dictionary<string, object>() { { "role", new List<string>() { "CompanyOwner" } } });
        
        Assert.AreEqual(0, result);
    }
    
    [TestMethod]
    public void GetPaginatedUsers_WhenNamesAreSpecified_ShouldReturnUsersWithSpecifiedNames()
    {
        var result = _userRepository.GetPaginated(1, 1, new Dictionary<string, object>() { { "fullName", new List<string>() { "John Doe" } } });
        
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void Count_WhenNamesAreSpecified_ShouldReturnTotalUsersWithSpecifiedNames()
    {
        var result = _userRepository.Count(new Dictionary<string, object>() { { "fullName", new List<string>() { "John Doe" } } });
        
        Assert.AreEqual(0, result);
    }
    
    [TestMethod]
    public void GetPaginatedUsers_WhenNamesAndRolesAreSpecified_ShouldReturnUsersWithSpecifiedNamesAndRoles()
    {
        var result = _userRepository.GetPaginated(1, 1, new Dictionary<string, object>() { { "fullName", new List<string>() { "John Doe" }}, { "role", new List<string>() { "CompanyOwner" } } });
        
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void Count_WhenNamesAndRolesAreSpecified_ShouldReturnTotalUsersWithSpecifiedNamesAndRoles()
    {
        var result = _userRepository.Count(new Dictionary<string, object>() { { "fullName", new List<string>() { "John Doe" }}, { "role", new List<string>() { "CompanyOwner" } } });
        
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ApplyFilters_WhenFilterDoesNotExist_ShouldReturnUsers()
    {
        var result = _userRepository.Count(new Dictionary<string, object>() { { "nonExistentFilter", new List<string>() { "John Doe" } } });
        
        Assert.AreEqual(_data.ToList().Count, result);
    }
    
    [TestMethod]
    public void GetByEmail_ShouldReturnUser()
    {
        _aUser.Setup(u => u.Email).Returns("email@example.com");
        var result = _userRepository.GetByEmail("email@example.com");

        Assert.AreEqual(_aUser.Object, result);
    }
    
    [TestMethod]
    public void ApplyFilterNameWithEmptyFilter_ShouldReturnUsers()
    {
        var result = _userRepository.GetPaginated(1, 1, new Dictionary<string, object>() { { "fullName", new List<string>() { } } });
        
        Assert.AreEqual(_data.ToList().Count, result.Count());
    }
    
    [TestMethod]
    public void ApplyFilterRoleWithEmptyFilter_ShouldReturnUsers()
    {
        var result = _userRepository.GetPaginated(1, 1, new Dictionary<string, object>() { { "role", new List<string>() { } } });
        
        Assert.AreEqual(_data.ToList().Count, result.Count());
    }

}