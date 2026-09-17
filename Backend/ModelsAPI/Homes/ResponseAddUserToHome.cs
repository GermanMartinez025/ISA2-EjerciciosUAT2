namespace ModelsAPI.Homes;

public class ResponseAddUserToHome
{
    
    public int HomeId { get; set; }
    public string Email { get; set; }
    
    public ResponseAddUserToHome()
    {
    }
    
    public ResponseAddUserToHome(int homeId, string email)
    {
        HomeId = homeId;
        Email = email;
    }
    
    
    
    
}