using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Common.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for automatic service registration
/// </summary>
public static class ServiceRegistrationExtensions
{
    /// <summary>
    /// Registers all services implementing IBaseService from the assembly containing the specified type
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="marker">A type whose assembly will be scanned for IBaseService implementations</param>
    /// <returns>The modified service collection</returns>
    public static IServiceCollection RegisterServicesFromAssemblyContaining(this IServiceCollection services, Type marker)
    {
        var assembly = marker.Assembly;
        
        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IBaseService)) && t is { IsClass: true, IsAbstract: false, IsInterface: false })
            .ToArray();

        foreach (var serviceType in serviceTypes)
        {
            var interfaces = serviceType.GetInterfaces()
                .Where(i => i != typeof(IBaseService) && i.IsAssignableTo(typeof(IBaseService)))
                .ToArray();

            foreach (var serviceInterface in interfaces)
            {
                services.TryAddScoped(serviceInterface, serviceType);
            }
        }

        return services;
    }
}
