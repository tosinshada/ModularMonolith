using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Database;

namespace Modules.Users.Database;

public class UsersDatabaseMigrator : IModuleDatabaseMigrator
{
    public async Task MigrateAsync(IServiceScope scope, CancellationToken cancellationToken = default)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
