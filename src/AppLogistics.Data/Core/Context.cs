using AppLogistics.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Reflection;

namespace AppLogistics.Data.Core;

public class Context : DbContext
{
    #region Administration
    protected DbSet<Role> Role { get; set; }
    protected DbSet<Account> Account { get; set; }
    protected DbSet<Permission> Permission { get; set; }
    protected DbSet<RolePermission> RolePermission { get; set; }
    #endregion

    #region Configuration
    protected DbSet<Activity> Activity { get; set; }
    protected DbSet<Afp> Afp { get; set; }
    protected DbSet<BranchOffice> BranchOffice { get; set; }
    protected DbSet<Carrier> Carrier { get; set; }
    protected DbSet<Client> Client { get; set; }
    protected DbSet<Country> Country { get; set; }
    protected DbSet<DocumentType> DocumentType { get; set; }
    protected DbSet<EducationLevel> EducationLevel { get; set; }
    protected DbSet<Eps> Eps { get; set; }
    protected DbSet<EthnicGroup> EthnicGroup { get; set; }
    protected DbSet<MaritalStatus> MaritalStatus { get; set; }
    protected DbSet<Novelty> Novelties { get; set; }
    protected DbSet<ServiceNovelty> ServiceNovelties { get; set; }
    protected DbSet<Product> Product { get; set; }
    protected DbSet<Sector> Sector { get; set; }
    protected DbSet<Sex> Sex { get; set; }
    protected DbSet<VehicleType> VehicleType { get; set; }
    #endregion

    #region Operation
    protected DbSet<Employee> Employee { get; set; }
    protected DbSet<Rate> Rate { get; set; }
    protected DbSet<Service> Service { get; set; }
    #endregion

    #region System
    protected DbSet<AuditLog> AuditLog { get; set; }
    #endregion

    protected Context() { }
    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (PropertyInfo property in entity.ClrType.GetProperties())
            {
                if (property.GetCustomAttribute<Components.Mvc.IndexAttribute>(false) is Components.Mvc.IndexAttribute index)
                {
                    modelBuilder.Entity(entity.ClrType).HasIndex(property.Name).IsUnique(index.IsUnique);
                }
            }
        }

        modelBuilder.Entity<Permission>().Property(model => model.Id).ValueGeneratedNever();
        foreach (IMutableForeignKey key in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
        {
            key.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
