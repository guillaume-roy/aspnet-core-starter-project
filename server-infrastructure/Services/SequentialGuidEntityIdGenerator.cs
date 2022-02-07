using Microsoft.EntityFrameworkCore.ValueGeneration;

using ServerDomain.Services;

namespace ServerInfrastructure.Services;

public class SequentialGuidEntityIdGenerator : IEntityIdGenerator
{
    public Guid GenerateId()
    {
        var generator = new SequentialGuidValueGenerator();
        return generator.Next(null);
    }
}