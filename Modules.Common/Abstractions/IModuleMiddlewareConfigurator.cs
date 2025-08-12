using Microsoft.AspNetCore.Builder;

namespace Modules.Common.Abstractions;

public interface IModuleMiddlewareConfigurator
{
    IApplicationBuilder Configure(IApplicationBuilder app);
}
