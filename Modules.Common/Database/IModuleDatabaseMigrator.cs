using Microsoft.Extensions.DependencyInjection;

namespace Modules.Common.Database;

public interface IModuleDatabaseMigrator
{
    Task MigrateAsync(IServiceScope scope, CancellationToken cancellationToken = default);
}
