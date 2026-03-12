using AppLogistics.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace AppLogistics.Tests;

public class TestContext : Context
{
    public DbSet<TestModel> TestModels { get; set; }

    public TestContext(DbContextOptions<Context> options) : base(options)
    {
    }
}
