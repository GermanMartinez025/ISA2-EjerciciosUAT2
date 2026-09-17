namespace ModelsAPI.Companies;

public class ResponseGetComapanies
{
    public List<ResponseCompany> Companies { get; set; }
    public int TotalCompanies { get; set; }
    public int actualPage { get; set; }
    public int totalPages { get; set; }
    public ResponseGetComapanies()
    {
        Companies = new List<ResponseCompany>();
        TotalCompanies = 0;
        actualPage = 0;
        totalPages = 0;
    }
}