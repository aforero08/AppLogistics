using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace AppLogistics.Data.Logging.Tests
{
    public class LoggableEntityTests
    {
        private EntityEntry<BaseModel> entry;
        private DbContext context;
        private TestModel model;

        public LoggableEntityTests()
        {
            using (DbContext tempContext = TestingContext.Create())
            {
                tempContext.Add(model = ObjectsFactory.CreateTestModel());
                tempContext.SaveChanges();
            }

            context = TestingContext.Create();
            entry = context.Entry<BaseModel>(model);
        }

        #region LoggableEntity(EntityEntry<BaseModel> entry)

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAddedEntity()
        {
            entry.State = EntityState.Added;

            AsssertProperties(entry.CurrentValues);
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForModifiedEntity()
        {
            string title = model.Title;
            entry.State = EntityState.Modified;
            entry.CurrentValues["Title"] = "Role";
            entry.OriginalValues["Title"] = "Role";

            LoggableProperty expected = new LoggableProperty(entry.Property("Title"), title);
            LoggableProperty actual = new LoggableEntity(entry).Properties.Single();

            Assert.Equal(expected.IsModified, actual.IsModified);
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAttachedEntity()
        {
            string title = model.Title;

            context = TestingContext.Create();
            context.Set<TestModel>().Attach(model);

            entry = context.Entry<BaseModel>(model);
            entry.OriginalValues["Title"] = "Role";
            entry.CurrentValues["Title"] = "Role";
            entry.State = EntityState.Modified;

            LoggableProperty expected = new LoggableProperty(entry.Property("Title"), title);
            LoggableProperty actual = new LoggableEntity(entry).Properties.Single();

            Assert.Equal(expected.IsModified, actual.IsModified);
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForDeletedEntity()
        {
            entry.State = EntityState.Deleted;

            AsssertProperties(entry.GetDatabaseValues());
        }

        [Fact]
        public void LoggableEntity_SetsAction()
        {
            entry.State = EntityState.Deleted;

            string actual = new LoggableEntity(entry).Action;
            string expected = entry.State.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityTypeName()
        {
            string actual = new LoggableEntity(entry).Name;
            string expected = typeof(TestModel).Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityId()
        {
            int actual = new LoggableEntity(entry).Id();
            int expected = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion LoggableEntity(EntityEntry<BaseModel> entry)

        #region ToString()

        [Fact]
        public void ToString_FormsEntityChanges()
        {
            StringBuilder changes = new StringBuilder();
            LoggableEntity loggableEntity = new LoggableEntity(entry);
            foreach (LoggableProperty property in loggableEntity.Properties)
            {
                changes.AppendFormat("{0}{1}", property, Environment.NewLine);
            }

            string actual = loggableEntity.ToString();
            string expected = changes.ToString();

            Assert.Equal(expected, actual);
        }

        #endregion ToString()

        #region Test helpers

        private void AsssertProperties(PropertyValues newValues)
        {
            LoggableProperty[] actual = new LoggableEntity(entry).Properties.ToArray();
            LoggableProperty[] expected = newValues.Properties.Where(property => property.Name != "Id")
                .Select(property => new LoggableProperty(entry.Property(property.Name), newValues[property])).ToArray();

            for (int i = 0; i < expected.Length || i < actual.Length; i++)
            {
                Assert.Equal(expected[i].IsModified, actual[i].IsModified);
                Assert.Equal(expected[i].ToString(), actual[i].ToString());
            }
        }

        #endregion Test helpers
    }
}
