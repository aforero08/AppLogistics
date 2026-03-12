using AppLogistics.Data.Logging;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppLogistics.Data.Core.Tests
{
    public class UnitOfWorkTests
    {
        private DbContext context;
        private UnitOfWork unitOfWork;
        private IAuditLogger logger;
        private TestModel model;

        public UnitOfWorkTests()
        {
            context = TestingContext.Create();
            logger = Substitute.For<IAuditLogger>();
            model = ObjectsFactory.CreateTestModel();
            unitOfWork = new UnitOfWork(context, TestingContext.Mapper, logger);
        }

        #region GetAs<TModel, TDestination>(Int32? id)

        [Fact]
        public void GetAs_Null_ReturnsDestinationDefault()
        {
            Assert.Null(unitOfWork.GetAs<TestModel, TestView>(null));
        }

        [Fact]
        public void GetAs_ReturnsModelAsDestinationModelById()
        {
            context.Add(model);
            context.SaveChanges();

            TestView expected = TestingContext.Mapper.Map<TestView>(model);
            TestView actual = unitOfWork.GetAs<TestModel, TestView>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion GetAs<TModel, TDestination>(Int32? id)

        #region Get<TModel>(Int32? id)

        [Fact]
        public void Get_Null_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<TestModel>(null));
        }

        [Fact]
        public void Get_ModelById()
        {
            context.Add(model);
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().AsNoTracking().Single();
            TestModel actual = unitOfWork.Get<TestModel>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Get_NotFound_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<TestModel>(0));
        }

        #endregion Get<TModel>(Int32? id)

        #region To<TDestination>(Object source)

        [Fact]
        public void To_ConvertsSourceToDestination()
        {
            TestView actual = unitOfWork.To<TestView>(model);
            TestView expected = TestingContext.Mapper.Map<TestView>(model);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion To<TDestination>(Object source)

        #region Select<TModel>()

        [Fact]
        public void Select_FromSet()
        {
            context.Add(model);
            context.SaveChanges();

            IEnumerable<TestModel> actual = unitOfWork.Select<TestModel>();
            IEnumerable<TestModel> expected = context.Set<TestModel>();

            Assert.Equal(expected, actual);
        }

        #endregion Select<TModel>()

        #region InsertRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void InsertRange_AddsModelsToDbSet()
        {
            IEnumerable<TestModel> models = new[] { ObjectsFactory.CreateTestModel(1), ObjectsFactory.CreateTestModel(2) };
            DbContext testingContext = Substitute.For<DbContext>();
            testingContext.When(sub => sub.AddRange(models)).DoNotCallBase();

            unitOfWork.Dispose();

            unitOfWork = new UnitOfWork(testingContext, TestingContext.Mapper);
            unitOfWork.InsertRange(models);

            testingContext.Received().AddRange(models);
        }

        #endregion InsertRange<TModel>(IEnumerable<TModel> models)

        #region Insert<TModel>(TModel model)

        [Fact]
        public void Insert_AddsModelToDbSet()
        {
            unitOfWork.Insert(model);

            object actual = context.ChangeTracker.Entries<TestModel>().Single().Entity;
            object expected = model;

            Assert.Equal(EntityState.Added, context.Entry(model).State);
            Assert.Same(expected, actual);
        }

        #endregion Insert<TModel>(TModel model)

        #region Update<TModel>(TModel model)

        [Theory]
        [InlineData(EntityState.Added, EntityState.Modified)]
        [InlineData(EntityState.Deleted, EntityState.Modified)]
        [InlineData(EntityState.Detached, EntityState.Modified)]
        [InlineData(EntityState.Modified, EntityState.Modified)]
        [InlineData(EntityState.Unchanged, EntityState.Unchanged)]
        public void Update_Entry(EntityState initialState, EntityState state)
        {
            EntityEntry<TestModel> entry = context.Entry(model);
            entry.State = initialState;

            unitOfWork.Update(model);

            EntityEntry<TestModel> actual = entry;

            Assert.Equal(state, actual.State);
            Assert.False(actual.Property(prop => prop.CreationDate).IsModified);
        }

        #endregion Update<TModel>(TModel model)

        #region DeleteRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void DeleteRange_Models()
        {
            IEnumerable<TestModel> models = new[] { ObjectsFactory.CreateTestModel(1), ObjectsFactory.CreateTestModel(2) };

            context.AddRange(models);
            context.SaveChanges();

            unitOfWork.DeleteRange(models);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion DeleteRange<TModel>(IEnumerable<TModel> models)

        #region Delete<TModel>(TModel model)

        [Fact]
        public void Delete_Model()
        {
            context.Add(model);
            context.SaveChanges();

            unitOfWork.Delete(model);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion Delete<TModel>(TModel model)

        #region Delete<TModel>(Int32 id)

        [Fact]
        public void Delete_ModelById()
        {
            context.Add(model);
            context.SaveChanges();

            unitOfWork.Delete<TestModel>(model.Id);
            unitOfWork.Commit();

            Assert.Empty(context.Set<TestModel>());
        }

        #endregion Delete<TModel>(Int32 id)

        #region Commit()

        [Fact]
        public void Commit_SavesChanges()
        {
            DbContext testingContext = Substitute.For<DbContext>();

            new UnitOfWork(testingContext, TestingContext.Mapper).Commit();

            testingContext.Received().SaveChanges();
        }

        [Fact]
        public void Commit_Logs()
        {
            unitOfWork.Commit();

            logger.Received().Log(Arg.Any<IEnumerable<EntityEntry<BaseModel>>>());
            logger.Received().Save();
        }

        [Fact]
        public void Commit_Failed_DoesNotSaveLogs()
        {
            logger.When(sub => sub.Log(Arg.Any<IEnumerable<EntityEntry<BaseModel>>>())).Do(call => throw new Exception());
            Exception exception = Record.Exception(() => unitOfWork.Commit());

            logger.Received().Log(Arg.Any<IEnumerable<EntityEntry<BaseModel>>>());
            logger.DidNotReceive().Save();
            Assert.NotNull(exception);
        }

        #endregion Commit()

        #region Dispose()

        [Fact]
        public void Dispose_Logger()
        {
            unitOfWork.Dispose();

            logger.Received().Dispose();
        }

        [Fact]
        public void Dispose_Context()
        {
            DbContext testingContext = Substitute.For<DbContext>();

            new UnitOfWork(testingContext, TestingContext.Mapper).Dispose();

            testingContext.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion Dispose()
    }
}
