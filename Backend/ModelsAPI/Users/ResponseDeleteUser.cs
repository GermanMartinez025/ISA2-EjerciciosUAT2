namespace ModelsAPI.Users;

public class ResponseDeleteUser
{
    public int Id { get; set; }
    
    public ResponseDeleteUser()
    {
        
    }
    public ResponseDeleteUser(int id)
    {
        Id = id;
    }
    
    
}