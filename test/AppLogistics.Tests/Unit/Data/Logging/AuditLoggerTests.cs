using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppLogistics.Data.Logging.Tests
{
    public class AuditLoggerTests
    {
        private EntityEntry<BaseModel> entry;
        private DbContext context;
        private AuditLogger logger;

        public AuditLoggerTests()
        {
            context = TestingContext.Create();
            logger = new AuditLogger(context, 1);
            DbContext dataContext = TestingContext.Create();
            TestModel model = ObjectsFactory.CreateTestModel();

            entry = dataContext.Entry<BaseModel>(dataContext.Add(model).Entity);
            dataContext.SaveChanges();
        }

        #region Log(IEnumerable<EntityEntry<BaseModel>> entries)

        [Fact]
        public void Log_Added()
        {
            entry.State = EntityState.Added;

            logger.Log(new[] { entry });
            logger.Save();

            LoggableEntity expected = new LoggableEntity(entry);
            AuditLog actual = context.Set<AuditLog>().Single();

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

            logger.Log(new[] { entry });
            logger.Save();

            LoggableEntity expected = new LoggableEntity(entry);
            AuditLog actual = context.Set<AuditLog>().Single();

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

            logger.Log(new[] { entry });
            logger.Save();

            Assert.Empty(context.Set<AuditLog>());
        }

        [Fact]
        public void Log_Deleted()
        {
            entry.State = EntityState.Deleted;

            logger.Log(new[] { entry });
            logger.Save();

            LoggableEntity expected = new LoggableEntity(entry);
            AuditLog actual = context.Set<AuditLog>().Single();

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

            logger.Log(new[] { entry });

            Assert.Empty(context.Set<AuditLog>());
        }

        #endregion Log(IEnumerable<EntityEntry<BaseModel>> entries)

        #region Log(LoggableEntity entity)

        [Fact]
        public void Log_Entity()
        {
            LoggableEntity entity = new LoggableEntity(entry);

            logger.Log(entity);
            logger.Save();

            AuditLog actual = context.Set<AuditLog>().Single();
            LoggableEntity expected = entity;

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

            logger.Log(new LoggableEntity(entry));

            Assert.Empty(context.Set<AuditLog>());
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

            logger.Log(entity);
            logger.Save();
            logger.Save();

            AuditLog actual = context.Set<AuditLog>().Single();
            LoggableEntity expected = entity;

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
            DbContext testingContext = Substitute.For<DbContext>();
            testingContext.ChangeTracker.Returns(context.ChangeTracker);

            new AuditLogger(testingContext, 0).Dispose();

            testingContext.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            logger.Dispose();
            logger.Dispose();
        }

        #endregion Dispose()
    }
}
