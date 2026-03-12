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
    public class CountryServiceTests
    {
        private CountryService service;
        private DbContext context;
        private Country country;

        public CountryServiceTests()
        {
            context = TestingContext.Create();
            service = new CountryService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<Country>().Add(country = ObjectsFactory.CreateCountry());
            context.SaveChanges();
        }

        #region Get<TView>(String id)

        [Fact]
        public void Get_ReturnsViewById()
        {
            CountryView actual = service.Get<CountryView>(country.Id);
            CountryView expected = TestingContext.Mapper.Map<CountryView>(country);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region GetViews()

        [Fact]
        public void GetViews_ReturnsCountryViews()
        {
            CountryView[] actual = service.GetViews().ToArray();
            CountryView[] expected = context
                .Set<Country>()
                .ProjectTo<CountryView>(TestingContext.Mapper.ConfigurationProvider)
                .OrderByDescending(view => view.CreationDate)
                .ToArray();

            for (int i = 0; i < expected.Length || i < actual.Length; i++)
            {
                Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Id, actual[i].Id);
            }
        }

        #endregion

        #region Create(CountryView view)

        [Fact]
        public void Create_Country()
        {
            CountryView view = ObjectsFactory.CreateCountryView(1);
            view.Id = 0;

            service.Create(view);

            Country actual = context.Set<Country>().AsNoTracking().Single(model => model.Id != country.Id);
            CountryView expected = view;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
        }

        #endregion

        #region Edit(CountryView view)

        [Fact]
        public void Edit_Country()
        {
            CountryView view = ObjectsFactory.CreateCountryView(country.Id);
            view.Name = "Name0";

            service.Edit(view);

            Country actual = context.Set<Country>().AsNoTracking().Single();
            Country expected = country;

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Delete(String id)

        [Fact]
        public void Delete_Country()
        {
            service.Delete(country.Id);

            Assert.Empty(context.Set<Country>());
        }

        #endregion
    }
}
