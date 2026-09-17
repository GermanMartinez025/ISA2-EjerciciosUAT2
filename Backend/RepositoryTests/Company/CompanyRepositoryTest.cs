using Microsoft.EntityFrameworkCore;
using ModelInterface.Devices;
using ModelInterface.Users.UserType;
using Moq;
using Models.Company;
using Models.Users;
using Repositories.CompanyRepositories;
using Models.Users.UserTypes;

namespace CompanyRepositoryTests;

    [TestClass]
    public class CompanyRepositoryTest
    {
        private Mock<DbSet<Company>> _mockSet;
        private Mock<DbContext> _mockContext;
        private CompanyRepository _repository;
        private List<Company> _companies;
        private IQueryable<Company> _data;

        [TestInitialize]
        public void Initialize()
        {
            _mockSet = new Mock<DbSet<Company>>();
            _mockContext = new Mock<DbContext>();

            var mockCompanyOwner1 = new Mock<User>();
            mockCompanyOwner1.Setup(m => m.FirstName).Returns("John");
            mockCompanyOwner1.Setup(m => m.LastName).Returns("Doe");
            mockCompanyOwner1.Setup(m => m.AddRole(It.IsAny<AUserType>()));

            
            var mockCompanyOwner2 = new Mock<User>();
            mockCompanyOwner2.Setup(m => m.FirstName).Returns("Jane");
            mockCompanyOwner2.Setup(m => m.LastName).Returns("Doe");
            mockCompanyOwner2.Setup(m => m.AddRole(It.IsAny<AUserType>()));
            

            _companies = new List<Company>
            {
                new Company { Id = 1, Name = "Company A", CompanyOwner = new CompanyOwner(mockCompanyOwner1.Object)},
                new Company { Id = 2, Name = "Company B", CompanyOwner = new CompanyOwner(mockCompanyOwner2.Object) }
            };
            

            _mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(_companies.AsQueryable().Provider);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(_companies.AsQueryable().Expression);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(_companies.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(_companies.AsQueryable().GetEnumerator());

            _mockContext.Setup(m => m.Set<Company>()).Returns(_mockSet.Object);

            
            _repository = new CompanyRepository(_mockContext.Object);
        }

        [TestMethod]
        public void Create_ShouldAddCompany()
        {
            var newCompany = new Company { Id = 3, Name = "Company C" };

            _repository.Create(newCompany);

            _mockSet.Verify(m => m.Add(It.IsAny<Company>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetById_ShouldReturnCompany_WhenExists()
        {
            var result = _repository.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Company A", result.Name);
        }

        [TestMethod]
        public void GetById_ShouldThrowException_WhenIdNotExists()
        {
            Assert.IsNull(_repository.GetById(99));
        }

        [TestMethod]
        public void GetById_ShouldIsNull_WhenIdIsZeroOrNegative()
        {
            Assert.IsNull(_repository.GetById(0));
            Assert.IsNull(_repository.GetById(-1));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCompanies()
        {
            var result = _repository.GetAll();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetPaginated_ShouldReturnPaginatedCompanies()
        {
            var result = _repository.GetPaginated(1, 1, null);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }

        [TestMethod]
        public void GetPaginated_ShouldApplyFilters()
        {
            var filters = new Dictionary<string, object>
            {
                { "companyName", new List<string> { "Company A" } }
            };

            var result = _repository.GetPaginated(1, 10, filters);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }

        [TestMethod]
        public void GetPaginated_ShouldReturnEmpty_WhenNoFilterMatches()
        {
            var filters = new Dictionary<string, object>
            {
                { "companyName", new List<string> { "Nonexistent Company" } }
            };

            var result = _repository.GetPaginated(1, 10, filters);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Count_ShouldReturnTotalCount()
        {
            var result = _repository.Count(null);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Count_ShouldApplyFilters()
        {
            var filters = new Dictionary<string, object>
            {
                { "companyName", new List<string> { "Company A" } }
            };

            var result = _repository.Count(filters);

            Assert.AreEqual(1, result);
        }
        
        [TestMethod]
        public void ApplyFilters_ShouldReturnSameQuery_WhenFiltersAreNull()
        {
            // Act
            var resultWithNullFilters = _repository.GetPaginated(1, 10, null);

            // Assert
            Assert.AreEqual(2, resultWithNullFilters.Count);
        }
        
        
        [TestMethod]
        public void ApplyFilters_ShouldReturnSameQuery_WhenFiltersAreEmpty()
        {
            // Act
            var resultWithEmptyFilters = _repository.GetPaginated(1, 10, new Dictionary<string, object>());

            // Assert
            Assert.AreEqual(2, resultWithEmptyFilters.Count);
        }

        [TestMethod]
        public void ApplyFilters_ShouldFilterByCompanyName()
        {
            // Arrange
            var filters = new Dictionary<string, object>
            {
                { "companyName", new List<string> { "Company A" } }
            };

            // Act
            var result = _repository.GetPaginated(1, 10, filters);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }

        [TestMethod]
        public void ApplyFilters_ShouldFilterByOwnerFullName()
        {
            // Arrange
            var filters = new Dictionary<string, object>
            {
                { "ownerFullName", new List<string> { "John Doe" } }
            };

            // Act
            var result = _repository.GetPaginated(1, 10, filters);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }

        [TestMethod]
        public void ApplyFilters_ShouldReturnEmpty_WhenNoMatchesForOwnerFullName()
        {
            // Arrange
            var filters = new Dictionary<string, object>
            {
                { "ownerFullName", new List<string> { "Nonexistent Owner" } }
            };

            // Act
            var result = _repository.GetPaginated(1, 10, filters);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FilterByCompanyName_ShouldReturnAll_WhenCompanyNamesIsEmpty()
        {
            // Arrange
            var emptyCompanyNames = new List<string>();

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "companyName", emptyCompanyNames } });

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void FilterByCompanyName_ShouldReturnEmpty_WhenNoCompanyMatches()
        {
            // Arrange
            var nonExistentCompanyNames = new List<string> { "Nonexistent Company" };

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "companyName", nonExistentCompanyNames } });

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FilterByCompanyName_ShouldReturnFilteredCompanies()
        {
            // Arrange
            var companyNames = new List<string> { "Company A" };

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "companyName", companyNames } });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }

        [TestMethod]
        public void FilterByOwnerCompanyName_ShouldReturnAll_WhenOwnerNamesIsEmpty()
        {
            // Arrange
            var emptyOwnerNames = new List<string>();

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "ownerFullName", emptyOwnerNames } });

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void FilterByOwnerCompanyName_ShouldReturnEmpty_WhenNoOwnerMatches()
        {
            // Arrange
            var nonExistentOwnerNames = new List<string> { "Nonexistent Owner" };

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "ownerFullName", nonExistentOwnerNames } });

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FilterByOwnerCompanyName_ShouldReturnFilteredCompanies()
        {
            // Arrange
            var ownerNames = new List<string> { "John Doe" };

            // Act
            var result = _repository.GetPaginated(1, 10, new Dictionary<string, object> { { "ownerFullName", ownerNames } });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Company A", result.First().Name);
        }
        
        [TestMethod]
        public void Update_ShouldUpdateCompany()
        {
            // Arrange
            var company = new Company { Id = 1, Name = "Company A" };

            // Act
            var result = _repository.Update(company);

            // Assert
            _mockSet.Verify(m => m.Update(It.IsAny<Company>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(company, result);
        }

    }

