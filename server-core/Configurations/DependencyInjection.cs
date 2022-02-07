using System.Reflection;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using ServerCore.Kernel.Authorizations;
using ServerCore.Services;

using ServerDomain.Services;

namespace ServerCore.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IUserEmailUniquenessChecker), typeof(UserEmailUniquenessChecker));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        return services;
    }
}