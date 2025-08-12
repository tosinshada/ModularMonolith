using ModularMonolith.Seeding;
using Modules.Common.Database;
using Modules.Common.Extensions;
using Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebHostDependencies();

builder.AddCoreHostLogging();

builder.Services.AddCoreWebApiInfrastructure();

// builder.Services.AddCoreInfrastructure(builder.Configuration,
// [
//     ShipmentsModuleRegistration.ActivityModuleName,
//     CarriersModuleRegistration.ActivityModuleName,
//     StocksModuleRegistration.ActivityModuleName
// ]);

builder.Services
    .AddUsersModule(builder.Configuration);

// Seed entities in DEVELOPMENT mode
// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddScoped<SeedService>();
// }

var app = builder.Build();

// Run migrations in DEVELOPMENT mode
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.MigrateModuleDatabasesAsync();

    var userSeedService = scope.ServiceProvider.GetRequiredService<UserSeedService>();
    await userSeedService.SeedUsersAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseModuleMiddlewares();

await app.RunAsync();
