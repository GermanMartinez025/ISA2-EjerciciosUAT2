using Mapper;

namespace MapperTests;

[TestClass]
public class MapperTests
{
    private Mapper<TestClass1, TestClass2> _mapper;
    private TestClass1 _testClass1;
    private TestClass2 _testClass2;

    [TestInitialize]
    public void Initialize()
    {
        _mapper = new Mapper<TestClass1, TestClass2>();
        _testClass1 = new TestClass1
        {
            Name = "Name",
            Value = 1.0,
            IsTrue = true,
            Date = new DateTime(2024, 2, 28)
        };
    }

    [TestMethod]
    public void MapUsingConstructor_ShouldMapProperties()
    {
        _testClass2 = _mapper.Convert(_testClass1);
        
        _testClass1.testMethod();
        
        Assert.AreEqual(_testClass1.Name, _testClass2.NameC);
        Assert.AreEqual(_testClass1.Value, _testClass2.ValueC);
        Assert.AreEqual(_testClass1.IsTrue, _testClass2.IsTrueC);
        Assert.AreEqual(_testClass1.Date.ToString("yyyy-MM-dd"), _testClass2.DateC);
    }

    [TestMethod]
    public void MapSpecificProperties_ShouldMapSpecificProperties()
    {
        _mapper
            .MapValues(t1 => t1.Name, t2 => t2.NameC)
            .MapValues(t1 => t1.Value, t2 => t2.ValueC);

        _testClass2 = _mapper.Convert(_testClass1);

        Assert.AreEqual(_testClass1.Name, _testClass2.NameC);
        Assert.AreEqual(_testClass1.Value, _testClass2.ValueC);
        Assert.IsFalse(_testClass2.IsTrueC);
        Assert.IsNull(_testClass2.DateC);
    }

    [TestMethod]
    public void MapSpecificPropertiesWithCast_ShouldMapSpecificPropertiesWithCast()
    {
        _mapper
            .MapValues(t1 => t1.Name, t2 => t2.NameC)
            .MapValues(t1 => t1.Value, t2 => t2.ValueC)
            .MapValues(t1 => t1.IsTrue, t2 => t2.IsTrueC)
            .MapValues(t1 => t1.Date, t2 => t2.DateC, v => ((DateTime)v).ToString("yyyy-MM-dd"));

        _testClass2 = _mapper.Convert(_testClass1);

        Assert.AreEqual(_testClass1.Name, _testClass2.NameC);
        Assert.AreEqual(_testClass1.Value, _testClass2.ValueC);
        Assert.AreEqual(_testClass1.IsTrue, _testClass2.IsTrueC);
        Assert.AreEqual(_testClass1.Date.ToString("yyyy-MM-dd"), _testClass2.DateC);
    }

    [TestMethod]
    public void MapSpecificPropertiesWithCastAndDefault_ShouldMapSpecificPropertiesWithCastAndDefault()
    {
        _testClass1.Name = null;
        _mapper
            .MapValues(t1 => t1.Name, t2 => t2.NameC, null, "Unknown")
            .MapValues(t1 => t1.Value, t2 => t2.ValueC)
            .MapValues(t1 => t1.IsTrue, t2 => t2.IsTrueC)
            .MapValues(t1 => t1.Date, t2 => t2.DateC, v => ((DateTime)v).ToString("yyyy-MM-dd"));

        _testClass2 = _mapper.Convert(_testClass1);

        Assert.AreEqual(_testClass2.NameC, "Unknown");
        Assert.AreEqual(_testClass1.Value, _testClass2.ValueC);
        Assert.AreEqual(_testClass1.IsTrue, _testClass2.IsTrueC);
        Assert.AreEqual(_testClass1.Date.ToString("yyyy-MM-dd"), _testClass2.DateC);
    }


    [TestMethod]
    public void MapSpecificUnexistingProperties_ShouldLogError()
    {
        _mapper
            .MapValues(t1 => t1.Name, t2 => t2.NameC)
            .MapValues(t1 => t1.Date, t2 => t2.DateC);

        _testClass2 = _mapper.Convert(_testClass1);
        Assert.AreEqual(_mapper.Errors.Count, 1);
        Assert.AreEqual(_mapper.Errors[0], "Error mapping Date to DateC: Object of type 'System.DateTime' cannot be converted to type 'System.String'.");
        
    }
   
    [TestMethod]
    public void MapUsingConstructor_ShouldLogError_WhenConversionFailsBecauseConstructorIsMissing()
    {
        var mapper = new Mapper<TestClass2, TestClass1>();
        var testClass2 = new TestClass2
        {
            NameC = "Name",
            ValueC = 1.0,
            IsTrueC = true,
            DateC = "2024-02-28"
        };
        Assert.ThrowsException<MissingMethodException>(() => mapper.Convert(testClass2));
        Assert.AreEqual(mapper.Errors.Count, 1);
    }
    
    [TestMethod]
    public void ShouldLogError_WhenRetrievingPropertyInformationFails()
    {
        _mapper
            .MapValues(t1 => t1.Name, t2 => t2.NameC)
            .MapValues(t1 => t1.testMethod, t2 => t2.DateC);

        _testClass2 = _mapper.Convert(_testClass1);
        Assert.AreEqual(_mapper.Errors.Count, 2);
        Assert.AreEqual(_mapper.Errors[0], "Error retrieving property information: Object reference not set to an instance of an object.");
        Assert.AreEqual(_mapper.Errors[1], "Error configuring property mapping: Object reference not set to an instance of an object.");
    }
    
    [TestMethod]
    public void MapUsingCustomMapper_ShouldMapProperties()
    {
        _mapper.UseCustomMapper(t1 => new TestClass2
        {
            NameC = t1.Name,
            ValueC = t1.Value,
            IsTrueC = t1.IsTrue,
            DateC = t1.Date.ToString("yyyy-MM-dd")
        });

        _testClass2 = _mapper.Convert(_testClass1);

        Assert.AreEqual(_testClass1.Name, _testClass2.NameC);
        Assert.AreEqual(_testClass1.Value, _testClass2.ValueC);
        Assert.AreEqual(_testClass1.IsTrue, _testClass2.IsTrueC);
        Assert.AreEqual(_testClass1.Date.ToString("yyyy-MM-dd"), _testClass2.DateC);
    }
}