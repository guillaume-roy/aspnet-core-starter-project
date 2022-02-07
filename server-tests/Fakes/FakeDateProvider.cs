using System;

using ServerDomain.Services;

namespace ServerTests.Fakes;

public class FakeDateProvider : IDateProvider
{
    private readonly DateTime _now;

    public DateTime Now => _now;

    public FakeDateProvider(DateTime now)
    {
        _now = now;
    }
}