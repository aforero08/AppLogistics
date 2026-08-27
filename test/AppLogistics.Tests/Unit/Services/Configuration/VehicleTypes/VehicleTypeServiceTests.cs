using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class VehicleTypeServiceTests
{
    private VehicleTypeService service;
    private DbContext context;
    private VehicleType vehicleType;

    public VehicleTypeServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new VehicleTypeService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<VehicleType>().Add(vehicleType = ObjectsFactory.CreateVehicleType());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        VehicleTypeView actual = service.Get<VehicleTypeView>(vehicleType.Id);
        VehicleTypeView expected = TestFixture.Mapper.Map<VehicleTypeView>(vehicleType);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsVehicleTypeViews()
    {
        VehicleTypeView[] actual = service.GetViews().ToArray();
        VehicleTypeView[] expected = context
            .Set<VehicleType>()
            .ProjectTo<VehicleTypeView>(TestFixture.Mapper.ConfigurationProvider)
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

    #region Create(VehicleTypeView view)

    [Fact]
    public void Create_VehicleType()
    {
        VehicleTypeView view = ObjectsFactory.CreateVehicleTypeView(1);
        view.Id = 0;

        service.Create(view);

        VehicleType actual = context.Set<VehicleType>().AsNoTracking().Single(model => model.Id != vehicleType.Id);
        VehicleTypeView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion Create(VehicleTypeView view)

    #region Edit(VehicleTypeView view)

    [Fact]
    public void Edit_VehicleType()
    {
        VehicleTypeView view = ObjectsFactory.CreateVehicleTypeView(vehicleType.Id);
        view.Name = "Name0";

        service.Edit(view);

        VehicleType actual = context.Set<VehicleType>().AsNoTracking().Single();
        VehicleType expected = vehicleType;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(VehicleTypeView view)

    #region Delete(String id)

    [Fact]
    public void Delete_VehicleType()
    {
        service.Delete(vehicleType.Id);

        Assert.Empty(context.Set<VehicleType>());
    }

    #endregion Delete(String id)
}
