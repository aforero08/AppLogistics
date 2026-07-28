using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class ServiceServiceTests
{
    private ServiceService serviceService;
    private DbContext context;
    private Service service;
    private Rate rate;

    public ServiceServiceTests()
    {
        context = TestFixture.Create().Drop();
        serviceService = new ServiceService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<Service>().Add(service = ObjectsFactory.CreateService());
        context.Set<Rate>().Add(rate = ObjectsFactory.CreateRate(1));
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact(Skip = "Need to set up related models properly")]
    public void Get_ReturnsViewById()
    {
        ServiceView actual = serviceService.Get<ServiceView>(service.Id);
        ServiceView expected = TestFixture.Mapper.Map<ServiceView>(service);

        Assert.Equal(expected.CustomsInformation, actual.CustomsInformation);
        Assert.Equal(expected.VehicleNumber, actual.VehicleNumber);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Comments, actual.Comments);
        Assert.Equal(expected.Location, actual.Location);
        Assert.Equal(expected.Quantity, actual.Quantity);
        Assert.Equal(expected.RateActivityName, actual.RateActivityName);
        Assert.Equal(expected.RateClientName, actual.RateClientName);
        Assert.Equal(expected.RateProductName, actual.RateProductName);
        Assert.Equal(expected.UnifiedVehicleTypeName, actual.UnifiedVehicleTypeName);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact(Skip = "Need to set up related models properly")]
    public void GetViews_ReturnsServiceViews()
    {
        ServiceView[] actual = serviceService.GetViews().ToArray();
        ServiceView[] expected = context
            .Set<Service>()
            .ProjectTo<ServiceView>(TestFixture.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CustomsInformation, actual[i].CustomsInformation);
            Assert.Equal(expected[i].VehicleNumber, actual[i].VehicleNumber);
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Comments, actual[i].Comments);
            Assert.Equal(expected[i].Location, actual[i].Location);
            Assert.Equal(expected[i].Quantity, actual[i].Quantity);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion

    #region Create(ServiceView view)

    [Fact(Skip = "Need to set up related models properly")]
    public void Create_Service()
    {
        ServiceCreateEditView view = ObjectsFactory.CreateServiceCreateEditView(1);
        view.Id = 0;

        serviceService.Create(view);

        Service actual = context.Set<Service>().AsNoTracking().Single(model => model.Id != service.Id);
        ServiceCreateEditView expected = view;

        Assert.Equal(expected.CustomsInformation, actual.CustomsInformation);
        Assert.Equal(expected.VehicleNumber, actual.VehicleNumber);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Comments, actual.Comments);
        Assert.Equal(expected.Location, actual.Location);
        Assert.Equal(expected.Quantity, actual.Quantity);
        Assert.Equal(expected.RateId, actual.RateId);
    }

    #endregion

    #region Edit(ServiceView view)

    [Fact(Skip = "Need to check execution order?")]
    public void Edit_Service()
    {
        ServiceCreateEditView view = ObjectsFactory.CreateServiceCreateEditView(service.Id);
        view.CustomsInformation = "test";

        serviceService.Edit(view);

        Service actual = context.Set<Service>().AsNoTracking().Single();
        Service expected = service;

        Assert.Equal(expected.CustomsInformation, actual.CustomsInformation);
        Assert.Equal(expected.VehicleNumber, actual.VehicleNumber);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.HoldingPrice, actual.HoldingPrice);
        Assert.Equal(expected.FullPrice, actual.FullPrice);
        Assert.Equal(expected.Comments, actual.Comments);
        Assert.Equal(expected.Location, actual.Location);
        Assert.Equal(expected.Quantity, actual.Quantity);
        Assert.Equal(expected.RateId, actual.RateId);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact(Skip = "Need to check execution order?")]
    public void Delete_Service()
    {
        serviceService.Delete(service.Id);

        Assert.Empty(context.Set<Service>());
    }

    #endregion
}
