namespace ServerDomain.Services;

public interface IDateProvider
{
    DateTime Now { get; }
}