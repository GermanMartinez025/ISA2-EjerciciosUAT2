namespace ModelsAPI.Users;

public class ResponseGetMembers
{
    public List<ResponseGetHomeMember> Members { get; set; } = new List<ResponseGetHomeMember>();

}