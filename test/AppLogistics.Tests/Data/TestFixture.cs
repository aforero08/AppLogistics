using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using MvcTemplate.Objects.Mapping;
using System;
using System.Linq;

namespace AppLogistics.Tests;

public static class TestFixture
{
    public static IMapper Mapper { get; }
    private static DbContextOptions<Context> Options { get; }
    private static SqliteConnection _connection;

    static TestFixture()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("configuration.testing.json", optional: true)
            .AddEnvironmentVariables("MvcTemplate__Testing__")
            .Build();
        Mapper = new MapperConfiguration(mapper =>
        {
            mapper.AddProfile(new MappingProfile());
            mapper.CreateMap<TestModel, TestView>();
        }, new NullLoggerFactory()).CreateMapper();

        string connectionString = config["Data:Connection"];
        var optionsBuilder = new DbContextOptionsBuilder<Context>();

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            Options = optionsBuilder.UseSqlServer(connectionString).Options;
        }
        else
        {
            // Use SQLite in-memory with case-insensitive collation to match SQL Server behavior
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Create a custom case-insensitive collation function
            _connection.CreateCollation("NOCASE", (x, y) =>
                string.Compare(x, y, StringComparison.OrdinalIgnoreCase));

            Options = optionsBuilder
                .UseSqlite(_connection)
                .Options;
        }

        using DbContext context = Create();
        if (!context.Database.IsSqlite() && context.Database.IsRelational())
        {
            context.Database.Migrate();
        }
    }

    public static DbContext Create()
    {
        DbContext context = new TestDbContext(Options);

        // EnsureDeleted is used by cache tests. Recreate the shared in-memory
        // SQLite schema for the next context without changing production mappings.
        if (context.Database.IsSqlite())
        {
            context.Database.EnsureCreated();
        }

        return context;
    }

    public static DbContext Drop(this DbContext context)
    {
        // Remove entities in dependency order (children before parents)
        // Wrap in try-catch to handle cases where tables don't exist (e.g., SQLite schema issues)

        void SafeRemove<T>() where T : class
        {
            try
            {
                context.RemoveRange(context.Set<T>());
            }
            catch (Microsoft.Data.Sqlite.SqliteException)
            {
                // Ignore table not found errors in SQLite
            }
        }

        // Test entities
        SafeRemove<TestModel>();

        // Operation entities
        SafeRemove<Service>();
        SafeRemove<Rate>();
        SafeRemove<Employee>();

        // Configuration entities
        SafeRemove<ServiceNovelty>();
        SafeRemove<VehicleType>();
        SafeRemove<Sex>();
        SafeRemove<Sector>();
        SafeRemove<Product>();
        SafeRemove<Novelty>();
        SafeRemove<MaritalStatus>();
        SafeRemove<EthnicGroup>();
        SafeRemove<Eps>();
        SafeRemove<EducationLevel>();
        SafeRemove<DocumentType>();
        SafeRemove<Country>();
        SafeRemove<Client>();
        SafeRemove<Carrier>();
        SafeRemove<BranchOffice>();
        SafeRemove<Afp>();
        SafeRemove<Activity>();

        // Administration entities
        SafeRemove<RolePermission>();
        SafeRemove<Permission>();
        SafeRemove<Account>();
        SafeRemove<Role>();

        // System entities
        SafeRemove<AuditLog>();

        context.SaveChanges();

        return context;
    }
    public static IQueryable<T> Db<T>(this DbContext context) where T : class
    {
        return context.Set<T>().AsNoTracking();
    }
}
