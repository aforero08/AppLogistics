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
    public class MaritalStatusServiceTests
    {
        private MaritalStatusService service;
        private DbContext context;
        private MaritalStatus maritalStatus;

        public MaritalStatusServiceTests()
        {
            context = TestingContext.Create();
            service = new MaritalStatusService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<MaritalStatus>().Add(maritalStatus = ObjectsFactory.CreateMaritalStatus());
            context.SaveChanges();
        }

        #region Get<TView>(String id)

        [Fact]
        public void Get_ReturnsViewById()
        {
            MaritalStatusView actual = service.Get<MaritalStatusView>(maritalStatus.Id);
            MaritalStatusView expected = TestingContext.Mapper.Map<MaritalStatusView>(maritalStatus);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Get<TView>(String id)

        #region GetViews()

        [Fact]
        public void GetViews_ReturnsMaritalStatusViews()
        {
            MaritalStatusView[] actual = service.GetViews().ToArray();
            MaritalStatusView[] expected = context
                .Set<MaritalStatus>()
                .ProjectTo<MaritalStatusView>(TestingContext.Mapper.ConfigurationProvider)
                .OrderByDescending(view => view.CreationDate)
                .ToArray();

            for (int i = 0; i < expected.Length || i < actual.Length; i++)
            {
                Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }

        #endregion GetViews()

        #region Create(MaritalStatusView view)

        [Fact]
        public void Create_MaritalStatus()
        {
            MaritalStatusView view = ObjectsFactory.CreateMaritalStatusView(1);
            view.Id = 0;

            service.Create(view);

            MaritalStatus actual = context.Set<MaritalStatus>().AsNoTracking().Single(model => model.Id != maritalStatus.Id);
            MaritalStatusView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
        }

        #endregion Create(MaritalStatusView view)

        #region Edit(MaritalStatusView view)

        [Fact]
        public void Edit_MaritalStatus()
        {
            MaritalStatusView view = ObjectsFactory.CreateMaritalStatusView(maritalStatus.Id);
            view.Name = "Name0";

            service.Edit(view);

            MaritalStatus actual = context.Set<MaritalStatus>().AsNoTracking().Single();
            MaritalStatus expected = maritalStatus;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Edit(MaritalStatusView view)

        #region Delete(String id)

        [Fact]
        public void Delete_MaritalStatus()
        {
            service.Delete(maritalStatus.Id);

            Assert.Empty(context.Set<MaritalStatus>());
        }

        #endregion Delete(String id)
    }
}
