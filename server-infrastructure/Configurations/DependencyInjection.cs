using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServerCore.Kernel.Repositories;

using ServerDomain.Services;

using ServerInfrastructure.Database;
using ServerInfrastructure.Database.Repositories;
using ServerInfrastructure.Services;

namespace ServerInfrastructure.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("Database:UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseInMemoryDatabase("PizzaDb")
                .LogTo(message => Debug.WriteLine(message)));
        }
        // else
        // {
        //     services.AddDbContext<ApplicationDbContext>(options =>
        //         options.UseSqlServer(
        //             configuration.GetConnectionString("DefaultConnection"),
        //             b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        // }

        services.AddScoped(typeof(IWriteRepository<>), typeof(EfWriteRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(EfUnitOfWork));
        services.AddScoped(typeof(IEntityIdGenerator), typeof(SequentialGuidEntityIdGenerator));
        services.AddScoped(typeof(IPasswordHasher), typeof(BCryptPasswordHasher));
        services.AddScoped(typeof(IDateProvider), typeof(DateProvider));

        return services;
    }
}