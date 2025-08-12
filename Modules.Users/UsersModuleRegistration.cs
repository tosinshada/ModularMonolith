using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Users;

public static class UsersModuleRegistration
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddUsersModuleApi()
            .AddUsersInfrastructure(configuration);
    }
    
    private static IServiceCollection AddUsersModuleApi(this IServiceCollection services)
    {
        // Register controllers
        services.AddControllers()
            .AddApplicationPart(typeof(UsersModuleRegistration).Assembly);
        
        // Register services automatically
        services.RegisterServicesFromAssemblyContaining(typeof(UsersModuleRegistration));
        
        services.AddValidatorsFromAssembly(typeof(UsersModuleRegistration).Assembly);

        return services;
    }
}