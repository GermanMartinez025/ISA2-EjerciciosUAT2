namespace MapperTests;

public class TestClass2
{
    public string NameC { get; set; }
    public double ValueC { get; set; }
    public bool IsTrueC { get; set; }
    public string DateC { get; set; }
    
    public TestClass2()
    {
    }
    
    public TestClass2(TestClass1 testClass1)
    {
        NameC = testClass1.Name;
        ValueC = testClass1.Value;
        IsTrueC = testClass1.IsTrue;
        DateC = testClass1.Date.ToString("yyyy-MM-dd");
    }
}