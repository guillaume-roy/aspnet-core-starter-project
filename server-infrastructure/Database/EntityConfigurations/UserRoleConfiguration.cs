using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ServerDomain.Entities.Users;

namespace ServerInfrastructure.Database.EntityConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
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

        builder.Property(x => x.Type)
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserId);
    }
}