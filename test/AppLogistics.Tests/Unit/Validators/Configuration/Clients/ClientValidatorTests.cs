using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ClientValidatorTests
{
    private ClientValidator validator;
    private DbContext context;
    private Client client;

    public ClientValidatorTests()
    {
        context = TestingContext.Create();
        validator = new ClientValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Client>().Add(client = ObjectsFactory.CreateClient());
        context.SaveChanges();
    }

    #region CanCreate(ClientView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateClientCreateEditView(1)));
    }

    [Fact]
    public void CanCreate_ValidClient()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateClientCreateEditView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(ClientView view)

    #region CanEdit(ClientView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateClientCreateEditView(client.Id)));
    }

    [Fact]
    public void CanEdit_ValidClient()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateClientCreateEditView(client.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(ClientView view)
}
