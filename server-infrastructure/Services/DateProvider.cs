using ServerDomain.Services;

namespace ServerInfrastructure.Services;

public class DateProvider : IDateProvider
{
    public DateTime Now => DateTime.Now;
}