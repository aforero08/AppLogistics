using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ServiceValidatorTests
{
    private ServiceValidator validator;
    private DbContext context;
    private Service service;
    private Rate rate;

    public ServiceValidatorTests()
    {
        context = TestingContext.Create();
        validator = new ServiceValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Service>().Add(service = ObjectsFactory.CreateService());
        context.Set<Rate>().Add(rate = ObjectsFactory.CreateRate(1));
        context.SaveChanges();
    }

    #region CanCreate(ServiceView view)

    [Fact(Skip = "Need to resolve according to added validations")]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateServiceCreateEditView()));
    }

    [Fact(Skip = "Track error!")]
    public void CanCreate_ValidService()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateServiceCreateEditView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(ServiceView view)

    [Fact(Skip = "Track error!")]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateServiceCreateEditView(service.Id)));
    }

    [Fact(Skip = "Track error!")]
    public void CanEdit_ValidService()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateServiceCreateEditView(service.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion
}
