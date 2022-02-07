namespace ServerCore.Kernel.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Not found")
    {
    }
}