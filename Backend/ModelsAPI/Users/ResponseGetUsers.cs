namespace ModelsAPI.Users;

public class ResponseGetUsers
{
    public List<ResponseGetUser> Users { get; set; }
    public int TotalUsers { get; set; }
    public int actualPage { get; set; }
    public int totalPages { get; set; }
    public ResponseGetUsers()
    {
        Users = new List<ResponseGetUser>();
        TotalUsers = 0;
        actualPage = 0;
        totalPages = 0;
    }
}