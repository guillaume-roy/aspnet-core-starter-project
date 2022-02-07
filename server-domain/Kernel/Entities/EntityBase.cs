using ServerDomain.Kernel.BusinessRules;
using ServerDomain.Kernel.Events;

namespace ServerDomain.Kernel.Entities;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastModificationDate { get; set; }
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    protected EntityBase() { }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    protected async static Task CheckRule(IBusinessRule rule)
    {
        if (await rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}