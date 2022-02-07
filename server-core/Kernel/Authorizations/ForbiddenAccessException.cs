namespace ServerCore.Kernel.Authorizations;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Forbidden access")
    {
    }
}