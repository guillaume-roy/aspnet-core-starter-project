using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ServerDomain.Entities.Users;

namespace ServerInfrastructure.Database.EntityConfigurations;

public class UserForgotPasswordRequestConfiguration : IEntityTypeConfiguration<UserForgotPasswordRequest>
{
    public void Configure(EntityTypeBuilder<UserForgotPasswordRequest> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);

        builder.Property(x => x.CreationDate)
            .IsRequired();

        builder.Property(x => x.LastModificationDate)
            .IsRequired(false);

        builder.Property(x => x.Token)
            .IsRequired();

        builder.Property(x => x.ExpirationDate)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.ForgotPasswordRequests)
            .HasForeignKey(x => x.UserId); ;
    }
}