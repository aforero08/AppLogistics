using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppLogistics.Data.Logging.Tests;

public class AuditLoggerTests
{
    private EntityEntry<BaseModel> entry;
    private DbContext context;
    private AuditLogger logger;

    public AuditLoggerTests()
    {
        context = TestFixture.Create();
        logger = new AuditLogger(context, 1);
        DbContext dataContext = TestFixture.Create();
        TestModel model = ObjectsFactory.CreateTestModel();

        entry = dataContext.Entry<BaseModel>(dataContext.Add(model).Entity);
        dataContext.SaveChanges();
    }

    #region Log(IEnumerable<EntityEntry<BaseModel>> entries)

    [Fact]
    public void Log_Added()
    {
        entry.State = EntityState.Added;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new[] { entry });
        logger.Save();

        LoggableEntity expected = new LoggableEntity(entry);
        AuditLog actual = context.Set<AuditLog>().OrderBy(l => l.Id).Last();

        Assert.Equal(before + 1, context.Set<AuditLog>().Count());

        Assert.Equal(expected.ToString(), actual.Changes);
        Assert.Equal(expected.Name, actual.EntityName);
        Assert.Equal(expected.Action, actual.Action);
        Assert.Equal(expected.Id(), actual.EntityId);
        Assert.Equal(1, actual.AccountId);
    }

    [Fact]
    public void Log_Modified()
    {
        (entry.Entity as TestModel).Title += "Test";
        entry.State = EntityState.Modified;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new[] { entry });
        logger.Save();

        LoggableEntity expected = new LoggableEntity(entry);
        AuditLog actual = context.Set<AuditLog>().OrderBy(l => l.Id).Last();

        Assert.Equal(before + 1, context.Set<AuditLog>().Count());

        Assert.Equal(expected.ToString(), actual.Changes);
        Assert.Equal(expected.Name, actual.EntityName);
        Assert.Equal(expected.Action, actual.Action);
        Assert.Equal(expected.Id(), actual.EntityId);
        Assert.Equal(1, actual.AccountId);
    }

    [Fact]
    public void Log_NoChanges_DoesNotLog()
    {
        entry.State = EntityState.Modified;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new[] { entry });
        logger.Save();

        Assert.Equal(before, context.Set<AuditLog>().Count());
    }

    [Fact]
    public void Log_Deleted()
    {
        entry.State = EntityState.Deleted;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new[] { entry });
        logger.Save();

        LoggableEntity expected = new LoggableEntity(entry);
        AuditLog actual = context.Set<AuditLog>().OrderBy(l => l.Id).Last();

        Assert.Equal(before + 1, context.Set<AuditLog>().Count());

        Assert.Equal(expected.ToString(), actual.Changes);
        Assert.Equal(expected.Name, actual.EntityName);
        Assert.Equal(expected.Action, actual.Action);
        Assert.Equal(expected.Id(), actual.EntityId);
        Assert.Equal(1, actual.AccountId);
    }

    [Fact]
    public void Log_UnsupportedState_DoesNotLog()
    {
        IEnumerable<EntityState> unsupportedStates = Enum
            .GetValues(typeof(EntityState))
            .Cast<EntityState>()
            .Where(state =>
                state != EntityState.Added
                && state != EntityState.Modified
                && state != EntityState.Deleted);

        foreach (EntityState usupportedState in unsupportedStates)
        {
            entry.State = usupportedState;
            logger.Log(new[] { entry });
        }

        Assert.Empty(context.ChangeTracker.Entries<AuditLog>());
    }

    [Fact]
    public void Log_DoesNotSaveChanges()
    {
        entry.State = EntityState.Added;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new[] { entry });

        Assert.Equal(before, context.Set<AuditLog>().Count());
    }

    #endregion Log(IEnumerable<EntityEntry<BaseModel>> entries)

    #region Log(LoggableEntity entity)

    [Fact]
    public void Log_Entity()
    {
        LoggableEntity entity = new LoggableEntity(entry);

        int before = context.Set<AuditLog>().Count();

        logger.Log(entity);
        logger.Save();

        AuditLog actual = context.Set<AuditLog>().OrderBy(l => l.Id).Last();
        LoggableEntity expected = entity;

        Assert.Equal(before + 1, context.Set<AuditLog>().Count());

        Assert.Equal(expected.ToString(), actual.Changes);
        Assert.Equal(expected.Name, actual.EntityName);
        Assert.Equal(expected.Action, actual.Action);
        Assert.Equal(expected.Id(), actual.EntityId);
        Assert.Equal(1, actual.AccountId);
    }

    [Fact]
    public void Log_DoesNotSave()
    {
        entry.State = EntityState.Added;

        int before = context.Set<AuditLog>().Count();

        logger.Log(new LoggableEntity(entry));

        Assert.Equal(before, context.Set<AuditLog>().Count());
    }

    #endregion Log(LoggableEntity entity)

    #region Save()

    [Theory]
    [InlineData(1)]
    [InlineData(null)]
    public void Save_LogsOnce(int? expectedAccountId)
    {
        LoggableEntity entity = new LoggableEntity(entry);
        logger = new AuditLogger(context, expectedAccountId);

        int before = context.Set<AuditLog>().Count();

        logger.Log(entity);
        logger.Save();
        logger.Save();

        AuditLog actual = context.Set<AuditLog>().OrderBy(l => l.Id).Last();
        LoggableEntity expected = entity;

        Assert.Equal(before + 1, context.Set<AuditLog>().Count());

        Assert.Equal(expectedAccountId, actual.AccountId);
        Assert.Equal(expected.ToString(), actual.Changes);
        Assert.Equal(expected.Name, actual.EntityName);
        Assert.Equal(expected.Action, actual.Action);
        Assert.Equal(expected.Id(), actual.EntityId);
    }

    #endregion Save()

    #region Dispose()

    [Fact]
    public void Dispose_Context()
    {
        DbContext TestFixture = Substitute.For<DbContext>();
        TestFixture.ChangeTracker.Returns(context.ChangeTracker);

        new AuditLogger(TestFixture, 0).Dispose();

        TestFixture.Received().Dispose();
    }

    [Fact]
    public void Dispose_MultipleTimes()
    {
        logger.Dispose();
        logger.Dispose();
    }

    #endregion Dispose()
}
