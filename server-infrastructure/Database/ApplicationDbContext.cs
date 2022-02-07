using System.Reflection;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ServerDomain.Entities.Users;
using ServerDomain.Kernel.Entities;
using ServerDomain.Kernel.Events;

namespace ServerInfrastructure.Database;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator _mediator;

    // Users
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationDate = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModificationDate = DateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<EntityBase>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _mediator.Publish(@event);
        }
    }
}
