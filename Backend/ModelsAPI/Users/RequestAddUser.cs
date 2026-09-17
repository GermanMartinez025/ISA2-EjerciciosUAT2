namespace ModelsAPI.Users;

public class RequestAddUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ProfilePhoto { get; set; }
    public string Role { get; set; }

}