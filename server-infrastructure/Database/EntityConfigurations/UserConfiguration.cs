using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ServerDomain.Entities.Users;

namespace ServerInfrastructure.Database.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);

        builder.Property(x => x.CreationDate)
            .IsRequired();

        builder.Property(x => x.LastModificationDate)
            .IsRequired(false);

        builder.Property(x => x.IsEnabled)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasMany(x => x.Roles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.ForgotPasswordRequests)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}