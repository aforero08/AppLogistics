using AppLogistics.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace AppLogistics.Tests;

public class TestDbContext : Context
{
    public DbSet<TestModel> TestModels { get; set; }

    public TestDbContext(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure case-insensitive collation for SQLite to match SQL Server behavior
        if (Database.IsSqlite())
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetCollation("NOCASE");
                    }
                }
            }
        }
    }
}
