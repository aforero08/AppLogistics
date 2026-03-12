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
    public class CarrierServiceTests
    {
        private CarrierService service;
        private DbContext context;
        private Carrier carrier;

        public CarrierServiceTests()
        {
            context = TestingContext.Create();
            service = new CarrierService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<Carrier>().Add(carrier = ObjectsFactory.CreateCarrier());
            context.SaveChanges();
        }

        #region Get<TView>(String id)

        [Fact]
        public void Get_ReturnsViewById()
        {
            CarrierView actual = service.Get<CarrierView>(carrier.Id);
            CarrierView expected = TestingContext.Mapper.Map<CarrierView>(carrier);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Get<TView>(String id)

        #region GetViews()

        [Fact]
        public void GetViews_ReturnsCarrierViews()
        {
            CarrierView[] actual = service.GetViews().ToArray();
            CarrierView[] expected = context
                .Set<Carrier>()
                .ProjectTo<CarrierView>(TestingContext.Mapper.ConfigurationProvider)
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

        #region Create(CarrierView view)

        [Fact]
        public void Create_Carrier()
        {
            CarrierView view = ObjectsFactory.CreateCarrierView(1);
            view.Id = 0;

            service.Create(view);

            Carrier actual = context.Set<Carrier>().AsNoTracking().Single(model => model.Id != carrier.Id);
            CarrierView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
        }

        #endregion Create(CarrierView view)

        #region Edit(CarrierView view)

        [Fact]
        public void Edit_Carrier()
        {
            CarrierView view = ObjectsFactory.CreateCarrierView(carrier.Id);
            view.Name = "Name0";
            view.Nit = "Nit0";

            service.Edit(view);

            Carrier actual = context.Set<Carrier>().AsNoTracking().Single();
            Carrier expected = carrier;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nit, actual.Nit);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion Edit(CarrierView view)

        #region Delete(String id)

        [Fact]
        public void Delete_Carrier()
        {
            service.Delete(carrier.Id);

            Assert.Empty(context.Set<Carrier>());
        }

        #endregion Delete(String id)
    }
}
