using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace HospitalSystem.Infrastructure.Persistence;

public class HospitalDbContextFactory : IDesignTimeDbContextFactory<HospitalDbContext>
{
    public const string DefaultConnectionString =
        "Host=localhost;Port=5432;Database=hospital;Username=hospital;Password=hospital";

    public HospitalDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("HOSPITALDB_CONNECTION") ?? DefaultConnectionString;

        return new HospitalDbContext(BuildOptions(connectionString));
    }

    public static DbContextOptions<HospitalDbContext> BuildOptions(string connectionString)
    {
        // Native enums only work when the driver knows them too, and since Npgsql 8 that
        // registration has to happen on a data source rather than a plain connection string.
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        PgEnums.MapAll(dataSourceBuilder);
        var dataSource = dataSourceBuilder.Build();

        return new DbContextOptionsBuilder<HospitalDbContext>()
            .UseNpgsql(dataSource, npgsql => npgsql.MigrationsAssembly(
                typeof(HospitalDbContextFactory).Assembly.FullName))
            .Options;
    }
}
