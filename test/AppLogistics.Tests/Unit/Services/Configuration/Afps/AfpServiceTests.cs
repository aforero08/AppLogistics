using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests
{
    public class AfpServiceTests
    {
        private AfpService service;
        private DbContext context;
        private Afp afp;

        public AfpServiceTests()
        {
            context = TestingContext.Create();
            service = new AfpService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<Afp>().Add(afp = ObjectsFactory.CreateAfp());
            context.SaveChanges();
        }

        #region Get<TView>(String id)

        [Fact]
        public void Get_ReturnsViewById()
        {
            AfpView actual = service.Get<AfpView>(afp.Id);
            AfpView expected = TestingContext.Mapper.Map<AfpView>(afp);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Get<TView>(String id)

        #region GetViews()

        [Fact]
        public void GetViews_ReturnsAfpViews()
        {
            AfpView[] actual = service.GetViews().ToArray();
            AfpView[] expected = context
                .Set<Afp>()
                .ProjectTo<AfpView>(TestingContext.Mapper.ConfigurationProvider)
                .OrderByDescending(view => view.CreationDate)
                .ToArray();

            for (int i = 0; i < expected.Length || i < actual.Length; i++)
            {
                Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Nit, actual[i].Nit);
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }

        #endregion GetViews()

        #region Create(AfpView view)

        [Fact]
        public void Create_Afp()
        {
            AfpView view = ObjectsFactory.CreateAfpView(1);
            view.Id = 0;

            service.Create(view);

            Afp actual = context.Set<Afp>().AsNoTracking().Single(model => model.Id != afp.Id);
            AfpView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
        }

        #endregion Create(AfpView view)

        #region Edit(AfpView view)

        [Fact]
        public void Edit_Afp()
        {
            AfpView view = ObjectsFactory.CreateAfpView(afp.Id);
            view.Name = "Name0";
            view.Nit = "Nit0";

            service.Edit(view);

            Afp actual = context.Set<Afp>().AsNoTracking().Single();
            Afp expected = afp;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Edit(AfpView view)

        #region Delete(String id)

        [Fact]
        public void Delete_Afp()
        {
            service.Delete(afp.Id);

            Assert.Empty(context.Set<Afp>());
        }

        #endregion Delete(String id)
    }
}
