namespace ModelException;


public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Unauthorized access.") { }
}