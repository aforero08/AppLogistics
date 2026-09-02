using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ClientValidatorTests
{
    private ClientValidator validator;
    private DbContext context;
    private Client client;

    public ClientValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new ClientValidator(new UnitOfWork(context, TestFixture.Mapper));

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
        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(client.Id);
        view.Name = client.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Client otherClient = ObjectsFactory.CreateClient();
        otherClient.Name = "OtherName";
        otherClient.Nit = "OtherNit";
        context.Set<Client>().Add(otherClient);
        context.SaveChanges();

        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(client.Id);
        view.Name = otherClient.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_ToSameNit()
    {
        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(client.Id);
        view.Name = client.Name;
        view.Nit = client.Nit;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_UsedNit_ReturnsFalse()
    {
        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(client.Id + 1);
        view.Name = "UnusedName";
        view.Nit = client.Nit;

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(ClientView view)
}
