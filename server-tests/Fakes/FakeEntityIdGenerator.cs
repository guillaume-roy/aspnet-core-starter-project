using System;

using ServerDomain.Services;

namespace ServerTests.Fakes;

public class FakeEntityIdGenerator : IEntityIdGenerator
{
    public Guid GenerateId()
    {
        return Guid.NewGuid();
    }
}