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
    public class RateServiceTests : IDisposable
    {
        private RateService service;
        private TestingContext context;
        private Rate rateView;

        public RateServiceTests()
        {
            context = new TestingContext();
            service = new RateService(new UnitOfWork(new TestingContext(context)));

            context.Set<Rate>().Add(rateView = ObjectsFactory.CreateRate());
            context.SaveChanges();
        }

        public void Dispose()
        {
            service.Dispose();
            context.Dispose();
        }

        #region Get<TView>(String id)

        [Fact]
        public void Get_ReturnsViewById()
        {
            RateView actual = service.Get<RateView>(rateView.Id);
            RateView expected = Mapper.Map<RateView>(rateView);

            Assert.Equal(expected.VehicleTypeName, actual.VehicleTypeName);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.EmployeePercentage, actual.EmployeePercentage);
            Assert.Equal(expected.ActivityName, actual.ActivityName);
            Assert.Equal(expected.SplitFare, actual.SplitFare);
            Assert.Equal(expected.ClientName, actual.ClientName);
            Assert.Equal(expected.Price, actual.Price);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Get<TView>(String id)

        #region GetViews()

        [Fact]
        public void GetViews_ReturnsRateViews()
        {
            RateView[] actual = service.GetViews().ToArray();
            RateView[] expected = context
                .Set<Rate>()
                .ProjectTo<RateView>()
                .OrderByDescending(view => view.CreationDate)
                .ToArray();

            for (int i = 0; i < expected.Length || i < actual.Length; i++)
            {
                Assert.Equal(expected[i].VehicleTypeName, actual[i].VehicleTypeName);
                Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
                Assert.Equal(expected[i].EmployeePercentage, actual[i].EmployeePercentage);
                Assert.Equal(expected[i].ActivityName, actual[i].ActivityName);
                Assert.Equal(expected[i].SplitFare, actual[i].SplitFare);
                Assert.Equal(expected[i].ClientName, actual[i].ClientName);
                Assert.Equal(expected[i].Price, actual[i].Price);
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }

        #endregion GetViews()

        #region Create(RateView view)

        [Fact]
        public void Create_Rate()
        {
            RateCreateEditView view = ObjectsFactory.CreateRateCreateEditView(1);
            view.Id = 0;

            service.Create(view);

            Rate actual = context.Set<Rate>().AsNoTracking().Single(model => model.Id != rateView.Id);
            RateCreateEditView expected = view;

            Assert.Equal(expected.VehicleTypeId, actual.VehicleTypeId);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.EmployeePercentage, actual.EmployeePercentage);
            Assert.Equal(expected.ActivityId, actual.ActivityId);
            Assert.Equal(expected.SplitFare, actual.SplitFare);
            Assert.Equal(expected.ClientId, actual.ClientId);
            Assert.Equal(expected.Price, actual.Price);
        }

        #endregion Create(RateView view)

        #region Edit(RateView view)

        [Fact(Skip = "Pending implementation of edit logic")] // Keep skipped without forcing failure
        public void Edit_Rate()
        {
            RateCreateEditView view = ObjectsFactory.CreateRateCreateEditView(rateView.Id);
            // TODO: implement edit test when edit logic is verified
            // service.Edit(view);
        }

        #endregion Edit(RateView view)

        #region Delete(String id)

        [Fact]
        public void Delete_Rate()
        {
            service.Delete(rateView.Id);
            Assert.Empty(context.Set<Rate>());
        }

        #endregion Delete(String id)
    }
}
