namespace ServerDomain.Services;

public interface IEntityIdGenerator
{
    Guid GenerateId();
}